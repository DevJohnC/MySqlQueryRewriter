using Antlr4.Runtime.Tree;

namespace MySqlQueryRewriter
{
	public class RequiredWhereConditionRule : RewriteRule
	{
		public RequiredWhereConditionRule(string condition)
		{
			Condition = condition;
		}

		public string Condition { get; }

		private bool _isEnabledForQuery = false;

		private bool _whereClauseOpened = false;

		private bool _whereClauseHasHangingBracket = false;

		private bool _whereClauseClosed = false;

		public override void ConfigureQueryVisitor(MySqlQueryVisitor queryVisitor)
		{
			//  queries that don't support a where condition
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.INSERT_SYMBOL, Disable);
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.CREATE_SYMBOL, Disable);
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.DROP_SYMBOL, Disable);

			//  queries that support a where condition
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.SELECT_SYMBOL, EnableAndResetForQuery);
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.UPDATE_SYMBOL, EnableAndResetForQuery);
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.DELETE_SYMBOL, EnableAndResetForQuery);

			//  replace the WHERE symbol if one occurs
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.WHERE_SYMBOL, WhereClauseBegin);

			//  symbols that indicate the time for a where clause has passed
			queryVisitor.AddTerminalNodeRewriter(-1, WhereClauseTerminator); // EOF
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.HAVING_SYMBOL, WhereClauseTerminator);
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.GROUP_SYMBOL, WhereClauseTerminator);
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.ORDER_SYMBOL, WhereClauseTerminator);
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.LIMIT_SYMBOL, WhereClauseTerminator);
		}

		private RewriteResult Disable(MySqlQueryWriter writer, ITerminalNode node)
		{
			_isEnabledForQuery = false;
			return RewriteResult.NoChanges;
		}

		private RewriteResult EnableAndResetForQuery(MySqlQueryWriter writer, ITerminalNode node)
		{
			_isEnabledForQuery = true;
			_whereClauseOpened = false;
			_whereClauseClosed = false;
			_whereClauseHasHangingBracket = false;
			return RewriteResult.NoChanges;
		}

		private RewriteResult WhereClauseBegin(MySqlQueryWriter writer, ITerminalNode node)
		{
			if (!_isEnabledForQuery)
				return RewriteResult.NoChanges;

			OpenWhereClause(writer, isWhereClauseToken: true);
			return RewriteResult.ReplacedSymbol;
		}

		private RewriteResult WhereClauseTerminator(MySqlQueryWriter writer, ITerminalNode node)
		{
			if (!_isEnabledForQuery)
				return RewriteResult.NoChanges;

			OpenWhereClause(writer, isWhereClauseToken: false);
			CloseWhereClause(writer);
			return RewriteResult.NoChanges;
		}

		private void OpenWhereClause(MySqlQueryWriter writer, bool isWhereClauseToken)
		{
			if (_whereClauseOpened)
				return;

			_whereClauseOpened = true;
			_whereClauseHasHangingBracket = isWhereClauseToken;
			if (isWhereClauseToken)
				writer.WriteSymbol($"WHERE {Condition} AND (");
			else
				writer.WriteSymbol($"WHERE {Condition} ");
		}

		private void CloseWhereClause(MySqlQueryWriter writer)
		{
			if (!_whereClauseOpened || _whereClauseClosed)
				return;

			_whereClauseClosed = true;
			if (_whereClauseHasHangingBracket)
				writer.WriteSymbol(")");
		}
	}
}

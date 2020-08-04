using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using parsers;
using System;

namespace MySqlQueryRewriter
{
	/// <summary>
	/// Visits parsed mysql query nodes.
	/// </summary>
	public class MySqlQueryVisitor : MySQLParserBaseListener
	{
		private readonly MySqlQueryWriter _writer;

		public MySqlQueryVisitor(MySqlQueryWriter writer)
		{
			_writer = writer ?? throw new ArgumentNullException(nameof(writer));
		}

		public override void VisitErrorNode([NotNull] IErrorNode node)
		{
			base.VisitErrorNode(node);
		}

		public override void EnterSelectStatement([NotNull] MySQLParser.SelectStatementContext context)
		{
			_writer.WriteSelect();
			base.EnterSelectStatement(context);
		}

		public override void EnterInsertStatement([NotNull] MySQLParser.InsertStatementContext context)
		{
			_writer.WriteInsert();
		}

		public override void EnterUpdateStatement([NotNull] MySQLParser.UpdateStatementContext context)
		{
			_writer.WriteUpdate();
		}

		public override void EnterDeleteStatement([NotNull] MySQLParser.DeleteStatementContext context)
		{
			_writer.WriteDelete();
		}

		public override void EnterIdentifier([NotNull] MySQLParser.IdentifierContext context)
		{
			_writer.WriteIdentifier(context.GetText());
		}

		public override void EnterQueryExpression([NotNull] MySQLParser.QueryExpressionContext context)
		{
			//  pseudo functions like COUNT end up here?
			base.EnterQueryExpression(context);
		}

		public override void EnterFunctionName([NotNull] MySQLParser.FunctionNameContext context)
		{
			base.EnterFunctionName(context);
		}

		public override void EnterFunctionParameter([NotNull] MySQLParser.FunctionParameterContext context)
		{
			base.EnterFunctionParameter(context);
		}

		public override void EnterFromClause([NotNull] MySQLParser.FromClauseContext context)
		{
			base.EnterFromClause(context);
		}

		public override void ExitFromClause([NotNull] MySQLParser.FromClauseContext context)
		{
			base.ExitFromClause(context);
		}

		public override void EnterTableName([NotNull] MySQLParser.TableNameContext context)
		{
			var tableName = context.GetText();
			base.EnterTableName(context);
		}

		public override void EnterFunctionCall([NotNull] MySQLParser.FunctionCallContext context)
		{
			base.EnterFunctionCall(context);
		}
	}
}

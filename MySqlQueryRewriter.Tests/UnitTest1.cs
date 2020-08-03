using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using parsers;
using System;
using Xunit;

namespace MySqlQueryRewriter.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var lexer = new parsers.MySQLLexer(
				Antlr4.Runtime.CharStreams.fromstring("select count(*)")
				);
			lexer.sqlMode = parsers.SqlMode.IgnoreSpace;
			var parser = new parsers.MySQLParser(new Antlr4.Runtime.CommonTokenStream(lexer));
			parser.BuildParseTree = true;
			parser.Interpreter.PredictionMode = PredictionMode.SLL;
			var parseTree = parser.query();

			var treeStr = parseTree.GetText();
			var treeTextStr = parseTree.ToStringTree();

			var listener = new TestListener();

			ParseTreeWalker.Default.Walk(listener, parseTree);
		}

		private class TestListener : parsers.MySQLParserBaseListener
		{
			//public override void VisitErrorNode([NotNull] IErrorNode node)
			//{
			//	base.VisitErrorNode(node);
			//}

			//public override void EnterOnError([NotNull] MySQLParser.OnErrorContext context)
			//{
			//	base.EnterOnError(context);
			//}

			public override void EnterSelectStatement([NotNull] MySQLParser.SelectStatementContext context)
			{
				base.EnterSelectStatement(context);
			}

			//public override void EnterSelectItemList([NotNull] MySQLParser.SelectItemListContext context)
			//{
			//	base.EnterSelectItemList(context);
			//}

			//public override void EnterSelectItem([NotNull] MySQLParser.SelectItemContext context)
			//{
			//	base.EnterSelectItem(context);
			//}

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

			public override void EnterIdentifier([NotNull] MySQLParser.IdentifierContext context)
			{
				base.EnterIdentifier(context);
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
}

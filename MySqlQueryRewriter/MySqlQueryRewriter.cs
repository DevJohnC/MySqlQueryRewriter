using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;
using System.IO;

namespace MySqlQueryRewriter
{
	public class MySqlQueryRewriter
	{
		public string Rewrite(string queryText, params RewriteRule[] rewriteRules)
		{
			var output = new StringWriter();
			var errorOutput = new StringWriter();

			var lexer = new parsers.MySQLLexer(
				Antlr4.Runtime.CharStreams.fromstring(queryText),
				output,
				errorOutput
				);
			lexer.sqlMode = parsers.SqlMode.IgnoreSpace;

			var parser = new parsers.MySQLParser(new Antlr4.Runtime.CommonTokenStream(lexer));
			parser.BuildParseTree = true;
			//parser.Interpreter.PredictionMode = PredictionMode.SLL;

			var parseTree = parser.query();

			var writer = new MySqlQueryWriter();
			var visitor = new MySqlQueryVisitor(writer);

			var treeTextStr = parseTree.ToStringTree();

			var outputStr = output.ToString();
			var errorStr = errorOutput.ToString();

			MySqlTreeWalker.Instance.Walk(visitor, parseTree);

			return writer.ToString();
		}
	}
}

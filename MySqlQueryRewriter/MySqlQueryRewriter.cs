namespace MySqlQueryRewriter
{
	public class MySqlQueryRewriter
	{
		public string Rewrite(string queryText, params RewriteRule[] rewriteRules)
		{
			var lexer = new parsers.MySQLLexer(
				Antlr4.Runtime.CharStreams.fromstring(queryText)
				);
			lexer.sqlMode = parsers.SqlMode.IgnoreSpace;

			var parser = new parsers.MySQLParser(new Antlr4.Runtime.CommonTokenStream(lexer));
			parser.BuildParseTree = true;
			//parser.Interpreter.PredictionMode = PredictionMode.SLL;

			var parseTree = parser.query();

			var writer = new MySqlQueryWriter();
			var visitor = new MySqlQueryVisitor(writer);

			foreach (var rewriteRule in rewriteRules)
			{
				rewriteRule.ConfigureQueryVisitor(visitor);
			}

			MySqlTreeWalker.Instance.Walk(visitor, parseTree);

			return writer.ToString();
		}
	}
}

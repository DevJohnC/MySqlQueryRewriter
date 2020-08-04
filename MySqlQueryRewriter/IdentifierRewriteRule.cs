using Antlr4.Runtime.Tree;

namespace MySqlQueryRewriter
{
	public class IdentifierRewriteRule : RewriteRule
	{
		public IdentifierRewriteRule(string identifier, string replacement)
		{
			Identifier = identifier;
			Replacement = replacement;
		}

		public string Identifier { get; }
		public string Replacement { get; }

		public override void ConfigureQueryVisitor(MySqlQueryVisitor queryVisitor)
		{
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.IDENTIFIER, RewriteIdentifier);
		}

		private RewriteResult RewriteIdentifier(MySqlQueryWriter writer, ITerminalNode node)
		{
			var identifier = node.GetText();
			if (string.Equals(identifier, Identifier, System.StringComparison.OrdinalIgnoreCase))
			{
				identifier = Replacement;
			}
			writer.WriteToken(identifier);
			return RewriteResult.HaltProcessing;
		}
	}
}

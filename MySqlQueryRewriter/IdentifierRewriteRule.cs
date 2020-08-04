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
	}
}

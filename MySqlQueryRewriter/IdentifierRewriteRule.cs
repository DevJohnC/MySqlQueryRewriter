using Antlr4.Runtime.Tree;

namespace MySqlQueryRewriter
{
	public class IdentifierRewriteRule : RewriteRule
	{
		public IdentifierRewriteRule(string identifier, string replacement, bool replaceBacktickQuotedIdentifiers = true)
		{
			Identifier = identifier;
			Replacement = replacement;
			_replaceBacktickQuotedIdentifiers = replaceBacktickQuotedIdentifiers;

			if (replaceBacktickQuotedIdentifiers)
			{
				_backtickQuotedIdentifier = $"`{identifier}`";
				_backtickQuotedReplacement = $"`{replacement}`";
			}
		}

		public string Identifier { get; }
		public string Replacement { get; }

		private readonly bool _replaceBacktickQuotedIdentifiers;

		private readonly string _backtickQuotedIdentifier = string.Empty;

		private readonly string _backtickQuotedReplacement = string.Empty;

		public override void ConfigureQueryVisitor(MySqlQueryVisitor queryVisitor)
		{
			queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.IDENTIFIER, RewriteIdentifier);
			if (_replaceBacktickQuotedIdentifiers)
			{
				queryVisitor.AddTerminalNodeRewriter(parsers.MySQLLexer.BACK_TICK_QUOTED_ID, RewriteBacktickQuotedIdentifier);
			}
		}

		private RewriteResult RewriteIdentifier(MySqlQueryWriter writer, ITerminalNode node)
		{
			if (string.Equals(node.GetText(), Identifier, System.StringComparison.OrdinalIgnoreCase))
			{
				writer.WriteSymbol(Replacement);
				return RewriteResult.ReplacedSymbol;
			}
			return RewriteResult.NoChanges;
		}

		private RewriteResult RewriteBacktickQuotedIdentifier(MySqlQueryWriter writer, ITerminalNode node)
		{
			if (string.Equals(node.GetText(), _backtickQuotedIdentifier, System.StringComparison.OrdinalIgnoreCase))
			{
				writer.WriteSymbol(_backtickQuotedReplacement);
				return RewriteResult.ReplacedSymbol;
			}
			return RewriteResult.NoChanges;
		}
	}
}

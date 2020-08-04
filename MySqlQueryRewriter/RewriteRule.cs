namespace MySqlQueryRewriter
{
	public enum RewriteResult
	{
		NoChanges,
		ReplacedSymbol
	}

	public abstract class RewriteRule
	{
		public abstract void ConfigureQueryVisitor(MySqlQueryVisitor queryVisitor);
	}
}

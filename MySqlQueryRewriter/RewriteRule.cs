namespace MySqlQueryRewriter
{
	public enum RewriteResult
	{
		ContinueProcessing,
		HaltProcessing
	}

	public abstract class RewriteRule
	{
		public abstract void ConfigureQueryVisitor(MySqlQueryVisitor queryVisitor);
	}
}

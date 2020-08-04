namespace MySqlQueryRewriter.Tests
{
	public static class QueryExtensions
	{
		public static string NormalizeQueryString(this string queryString)
		{
			return queryString
				.Replace(" ", "")
				.Replace("\r", "")
				.Replace("\n", "")
				.ToLowerInvariant();
		}
	}
}

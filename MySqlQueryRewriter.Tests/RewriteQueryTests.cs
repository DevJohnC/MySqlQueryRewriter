using Xunit;

namespace MySqlQueryRewriter.Tests
{
	public class RewriteQueryTests
	{
		[Fact]
		public void Can_Rewrite_Identifiers()
		{
			var originalQueryText = @"SELECT   wp_posts.ID FROM wp_posts  LEFT JOIN wp_postmeta ON (wp_posts.ID = wp_postmeta.post_id AND wp_postmeta.meta_key = '_customize_restore_dismissed' ) WHERE 1=1  AND wp_posts.post_author IN (1)  AND ( 
  wp_postmeta.post_id IS NULL
) AND wp_posts.post_type = 'customize_changeset' AND((wp_posts.post_status = 'auto-draft')) GROUP BY wp_posts.ID ORDER BY wp_posts.post_date DESC LIMIT 0, 1";

			var queryRewriter = new MySqlQueryRewriter();

			var rewrittenQueryText = queryRewriter.Rewrite(originalQueryText,
				new IdentifierRewriteRule("wp_posts", "the_posts"));
		}
	}
}

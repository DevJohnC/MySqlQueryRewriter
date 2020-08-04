using Xunit;

namespace MySqlQueryRewriter.Tests
{
	public class RewriteQueryTests
	{
		[Fact]
		public void Can_Rewrite_Identifiers()
		{
			var originalQueryText = @"SELECT   `wp_posts`.ID FROM wp_posts  LEFT JOIN wp_postmeta ON (wp_posts.ID = wp_postmeta.post_id AND wp_postmeta.meta_key = '_customize_restore_dismissed' ) WHERE 1=1  AND wp_posts.post_author IN (1)  AND ( 
  wp_postmeta.post_id IS NULL
) AND wp_posts.post_type = 'customize_changeset' AND((wp_posts.post_status = 'auto-draft')) GROUP BY wp_posts.ID ORDER BY wp_posts.post_date DESC LIMIT 0, 1";

			var queryRewriter = new MySqlQueryRewriter();

			var rewrittenQueryText = queryRewriter.Rewrite(originalQueryText,
				new IdentifierRewriteRule("wp_posts", "the_posts"));

			Assert.Equal(
				originalQueryText.Replace("wp_posts", "the_posts").NormalizeQueryString(),
				rewrittenQueryText.NormalizeQueryString()
				);
		}

		[Fact]
		public void Can_Rewrite_Table_And_Add_BlogId_Condition()
		{
			var originalQueryText = @"SELECT   wp_3_posts.ID FROM wp_3_posts  LEFT JOIN wp_3_postmeta ON (wp_3_posts.ID = wp_3_postmeta.post_id AND wp_3_postmeta.meta_key = '_customize_restore_dismissed' ) WHERE 1=1  AND wp_3_posts.post_author IN (1)  AND ( 
  wp_3_postmeta.post_id IS NULL
) AND wp_3_posts.post_type = 'customize_changeset' AND((wp_3_posts.post_status = 'auto-draft')) GROUP BY wp_3_posts.ID ORDER BY wp_3_posts.post_date DESC LIMIT 0, 1";

			var queryRewriter = new MySqlQueryRewriter();

			var rewrittenQueryText = queryRewriter.Rewrite(originalQueryText,
				new IdentifierRewriteRule("wp_3_posts", "wp_posts"),
				new IdentifierRewriteRule("wp_3_postmeta", "wp_postmeta"),
				new RequiredWhereConditionRule("`wp_posts`.`blog_id` = 3")
				);

			Assert.Equal(
				"SELECT wp_posts . ID FROM wp_posts LEFT JOIN wp_postmeta ON ( wp_posts . ID = wp_postmeta . post_id AND wp_postmeta . meta_key = '_customize_restore_dismissed' ) WHERE `wp_posts`.`blog_id` = 3 AND ( 1 = 1 AND wp_posts . post_author IN ( 1 ) AND ( wp_postmeta . post_id IS NULL ) AND wp_posts . post_type = 'customize_changeset' AND ( ( wp_posts . post_status = 'auto-draft' ) ) ) GROUP BY wp_posts . ID ORDER BY wp_posts . post_date DESC LIMIT 0 , 1 ",
				rewrittenQueryText
				);
		}

		[Fact]
		public void Can_Rewrite_Table_And_Add_BlogId_Condition_Without_Original_Where_Clause()
		{
			var originalQueryText = @"SELECT   wp_3_posts.ID FROM wp_3_posts  LEFT JOIN wp_3_postmeta ON (wp_3_posts.ID = wp_3_postmeta.post_id AND wp_3_postmeta.meta_key = '_customize_restore_dismissed' ) GROUP BY wp_3_posts.ID ORDER BY wp_3_posts.post_date DESC LIMIT 0, 1";

			var queryRewriter = new MySqlQueryRewriter();

			var rewrittenQueryText = queryRewriter.Rewrite(originalQueryText,
				new IdentifierRewriteRule("wp_3_posts", "wp_posts"),
				new IdentifierRewriteRule("wp_3_postmeta", "wp_postmeta"),
				new RequiredWhereConditionRule("`wp_posts`.`blog_id` = 3")
				);

			Assert.Equal(
				"SELECT wp_posts . ID FROM wp_posts LEFT JOIN wp_postmeta ON ( wp_posts . ID = wp_postmeta . post_id AND wp_postmeta . meta_key = '_customize_restore_dismissed' ) WHERE `wp_posts`.`blog_id` = 3  GROUP BY wp_posts . ID ORDER BY wp_posts . post_date DESC LIMIT 0 , 1 ",
				rewrittenQueryText
				);
		}
	}
}

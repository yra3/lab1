using System;
using System.Threading.Tasks;
namespace AppModel;
public partial record AppClient {
	public async Task<bool> AddArticleBookmark(Guid articleId) {
		await using var cursor = await db.Connect(readWrite: true);
		var affected = await cursor.Execute(
			@"
INSERT INTO
	article_bookmark (
		article_id,
		user_id,
		creation_time
	)
	SELECT :article_id, :user_id, :current_time
	WHERE
	EXISTS (select 1 from article where id = :article_id)
	AND
	EXISTS (select 1 from app_user where id = :user_id)
	AND
	NOT EXISTS (select 1 from article_bookmark where article_id = :article_id and user_id = :user_id)
",
			new() {
				{ "article_id", articleId },
				{ "user_id", CurrentUserId},
				{"current_time", GetCurrentTime()},
			}
		);
		if (affected != 1) {
			return false;
		}
		await cursor.Commit();
		return true;
	}
}

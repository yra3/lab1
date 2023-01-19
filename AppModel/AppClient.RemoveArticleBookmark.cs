using System;
using System.Threading.Tasks;
namespace AppModel;
public partial record AppClient {
	public async Task<bool> RemoveArticleBookmark(Guid articleId) {
		await using var cursor = await db.Connect(readWrite: true);
		var affected = await cursor.Execute(
			@"
DELETE FROM
	article_bookmark
WHERE
	article_id = :article_id
AND
	user_id = :user_id
",
			new() {
				{ "article_id", articleId },
				{"user_id", CurrentUserId},
			}
		);
		if (affected == 0) {
			return false;
		}
		await cursor.Commit();
		return true;
	}
}

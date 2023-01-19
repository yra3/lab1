using System;
namespace AppModel;
public record Bookmark {
	public User Owner { get; init; } = User.Empty;
	public Article Article { get; init; } = Article.Empty;
	public DateTime CreationTime { get; init; }
}

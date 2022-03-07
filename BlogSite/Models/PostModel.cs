using BlogSite.Data;

namespace BlogSite.Models;

public class PostModel
{
    public int PostId { get; set; }
    public string Title { get; private set; }
    public string Contents { get; private set; }

    public PostModel(int postId)
    {
        PostId = postId;
        BlogPostController blogController = new BlogPostController();
        Title = blogController.GetTitleOfPost(postId);
        Contents = blogController.GetContentOfPost(postId);
    }
}
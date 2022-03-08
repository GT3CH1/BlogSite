namespace BlogSite.Controllers;

public interface IBlogPostController
{
    public int GetNumberOfPosts();
    public string GetContentOfPost(int postId);
    public List<int> GetListOfPostIds();
    public string GetTitleOfPost(int postId);
    public string StripHtmlTags(string html);
    public bool CreatePost(string postTitle, string postContent);
}
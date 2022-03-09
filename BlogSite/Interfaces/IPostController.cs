using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;


public interface IPostController
{
    /// <summary>
    /// Returns a page showing all of the posts
    /// </summary>
    /// <returns></returns>
    public IActionResult Index();

    /// <summary>
    /// Shows the Create Post view
    /// </summary>
    /// <returns></returns>
    public IActionResult CreatePost();

    /// <summary>
    /// Creates a new blog post from the given title and contents.
    /// </summary>
    /// <param name="title">The title of the blog post.</param>
    /// <param name="content">The content of the blog post.</param>
    /// <returns></returns>
    public IActionResult CreatePost(string title, string content);


    /// <summary>
    /// Shows the title and content from a given post.
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    public IActionResult PostsView(int postId);
}
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public interface IPostController
{
    public IActionResult Index();
    public IActionResult CreatePost();
    public IActionResult PostsView(int postId);
}
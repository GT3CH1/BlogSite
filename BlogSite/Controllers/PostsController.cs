using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public class PostsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [Route("/Posts/{postId}")]
    public IActionResult PostsView(int postId)
    {
        PostModel post = new PostModel(postId);
        ViewBag.Message = post;
        return View();
    }
}
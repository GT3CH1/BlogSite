using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public class PostsController : Controller, IPostController
{
    // GET
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult CreatePost()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CreatePost(string blogtitle, string blogpost)
    {
        var blogController = new BlogPostController();
        blogController.CreatePost(blogtitle, blogpost);
        return Index();
    }

    [Route("/Posts/View/{postId}")]
    public IActionResult PostsView(int postId)
    {
        PostModel post = new PostModel(postId);
        ViewBag.Message = post;
        return View();
    }
}
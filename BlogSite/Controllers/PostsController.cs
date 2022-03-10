using BlogSite.Data;
using BlogSite.Models;
using BlogSite.Views.Posts;
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
    public IActionResult CreatePost(string title, string content)
    {
        var postModel = new PostModel(title, content);
        postModel.Create();
        return View("Index");
    }

    [Route("/Posts/View/{postId}")]
    public IActionResult PostsView(int postId)
    {
        var post = PostDatabaseModel.GetPostById(postId);
        ViewBag.Message = post;
        return View();
    }


    [Route("/Posts/Edit/{postId}")]
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult EditPost(int postId)
    {
        var post = PostDatabaseModel.GetPostById(postId);
        ViewBag.Message = post;
        return View();
    }

    [Route("/Posts/Edit/{postId}")]
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult EditPost(string title, string content, int postId)
    {
        var post = new PostModel(title, content, postId);
        post.Update();
        return PostsView(postId);
    }

    [Route("/Posts/Delete/{postId}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeletePost(int postId)
    {
        var post = PostDatabaseModel.GetPostById(postId);
        post.Delete();
        return View("Index");
    }
}
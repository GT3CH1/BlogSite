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
        try
        {
            int newPostId = PostDatabaseController.CreatePost(postModel);
            return RedirectToAction("PostsView", new { postId = newPostId });
        }
        catch (ArgumentException)
        {
            return View("Error");
        }
    }

    [Route("/Posts/View/{postId}")]
    public IActionResult PostsView(int postId)
    {
        try
        {
            var post = PostDatabaseController.GetPostById(postId);
            ViewBag.Message.Title = post.Title;
            ViewBag.Message.Content = post.Content;
            return View();
        }
        catch (ArgumentException)
        {
            return View("Error");
        }
    }


    [Route("/Posts/Edit/{postId}")]
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult EditPost(int postId)
    {
        try
        {
            var post = PostDatabaseController.GetPostById(postId);
            ViewBag.Message.Title = post.Title;
            ViewBag.Message.Content = post.Content;
            return View();
        }
        catch (ArgumentException)
        {
            return View("Error");
        }
    }

    [Route("/Posts/Edit/{postId}")]
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult EditPost(string title, string content, int postId)
    {
        var post = new PostModel(title, content, postId);
        try
        {
            PostDatabaseController.EditPost(post);
            return RedirectToAction("PostsView", new { postId = postId });
        }
        catch (ArgumentException)
        {
            return View("Error");
        }
    }

    [Route("/Posts/Delete/{postId}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeletePost(int postId)
    {
        try
        {
            var post = PostDatabaseController.GetPostById(postId);
            PostDatabaseController.DeletePost(post);
            return View("Index");
        }
        catch (ArgumentException)
        {
            return View("Error");
        }
    }
}
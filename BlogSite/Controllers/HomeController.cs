using System.Diagnostics;
using System.Text.RegularExpressions;
using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public class HomeController : Controller, IHomeController
{
    private readonly ILogger<HomeController> _logger;
    private PostDbContext _context;

    public HomeController(ILogger<HomeController> logger, PostDbContext context)
    {
        _logger = logger;
        _context = context;
        ;
    }

    public IActionResult Index(string searchString)
    {
        var posts = from p in _context.Posts
            select p;

        if (!string.IsNullOrEmpty(searchString))
        {
            // Normalize the search string
            posts = posts.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()));
        }

        return View(posts.ToList());
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (!String.IsNullOrEmpty(HttpContext.Request.Query["search"]))
        {
            var searchString = HttpContext.Request.Query["search"];
            var posts = _context.Posts.Where(s => s.Title.Contains(searchString));
            ViewBag.search = searchString;
            return View(posts.ToList().GetRange(0, Math.Min(posts.Count(), 10)));
        }

        return View(_context.Posts.ToList().GetRange(0, Math.Min(_context.Posts.Count(), 10)));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Strips the html tags of the given string.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string StripHtmlTags(string html)
    {
        var noHtmlTags = Regex.Replace(html, "<.*?>", string.Empty);
        return Regex.Replace(noHtmlTags, "&.*?;", string.Empty);
    }
}
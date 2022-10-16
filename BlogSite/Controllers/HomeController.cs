/*
 * Copyright (c) 2022. Gavin Pease and contributors.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT
 * OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using System.Diagnostics;
using System.Text.RegularExpressions;
using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers;

public class HomeController : Controller, IHomeController
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
        ;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // get all posts and include "Author"
        var posts = _context.Posts.Include(p => p.Author).ToList();
        string searchString = "";
        if (!string.IsNullOrEmpty(HttpContext.Request.Query["search"]))
        {
            searchString = HttpContext.Request.Query["search"];
            ViewBag.search = searchString;
        }

        if (searchString != "")
            posts = posts.Where(p => p.Title.Contains(searchString) || p.Content.Contains(searchString)).ToList();
        if (!User.IsInRole("Admin"))
            posts = posts.Where(p => p.IsDraft == false).ToList();
        var range = Math.Min(posts.Count(), 10);
        return View(posts.GetRange(0, range));
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
        var noHtmlTags = Regex.Replace(html, "<[^>]*>*", string.Empty);
        return Regex.Replace(noHtmlTags, "&.*?;", string.Empty);
    }
}
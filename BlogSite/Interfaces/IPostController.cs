/*
 * Copyright (c) 2022. Gavin Pease.
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

    /// <summary>
    /// Allows a user to edit a post.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="postId"></param>
    /// <returns></returns>
    public IActionResult EditPost(string title, string content, int postId);

    /// <summary>
    /// Renders the post title + content from the given ID to allow updating.
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    public IActionResult EditPost(int postId);

    /// <summary>
    /// Deletes a post.
    /// </summary>
    /// <param name="postId"></param>
    public IActionResult DeletePost(int postId);
}
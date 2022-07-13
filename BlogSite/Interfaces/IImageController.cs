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

public interface IImageController
{
    /// <summary>
    /// Returns the index page of the image controller.
    /// </summary>
    /// <returns></returns>
    public IActionResult Index();

    /// <summary>
    /// Gets an image from the given file name.
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public IActionResult GetImage(string imageName);

    /// <summary>
    /// Uploads the image to the server.
    /// </summary>
    /// <param name="fileData">The base64 encoded version of the image.</param>
    /// <param name="fileName">The name of the image.</param>
    /// <returns></returns>
    public string UploadImage(string fileData, string fileName);
}
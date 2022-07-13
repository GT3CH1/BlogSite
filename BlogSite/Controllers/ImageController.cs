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

using BlogSite.Models;
using BlogSite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogSite.Controllers;

public class ImageController : Controller, IImageController
{
    public IActionResult Index()
    {
        throw new NotImplementedException();
    }

    [Route("/Image/Get/{imageName}")]
    [HttpGet]
    public IActionResult GetImage(string imageName)
    {
        // Get current directory
        var currentDirectory = Directory.GetCurrentDirectory();
        // Add Media path to current directory
        var mediaPath = Path.GetFullPath(Path.Combine(currentDirectory, "Media", imageName));
        // Check if file exists
        if (System.IO.File.Exists(mediaPath))
        {
            // Return file
            return PhysicalFile(mediaPath, "image/jpeg");
        }
        else
        {
            // Return default image
            return PhysicalFile(Path.GetFullPath(Path.Combine(currentDirectory, "Media", "default.jpg")), "image/jpeg");
        }
    }


    [Route("/Image/UploadImage")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public string UploadImage(string fileData, string fileName)
    {
        //Create new PhotoService
        var photoService = new PhotoService();
        string filePath;
        // Upload filedata using photoService
        var result = photoService.Upload(fileData, fileName, out filePath);
        // Create new location object.
        var location = new ImageLocation
        {
            ImagePath = filePath
        };
        // Return result as json 
        var res = JsonConvert.SerializeObject(location);
        return res;
    }
}
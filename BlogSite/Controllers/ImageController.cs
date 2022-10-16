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

using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public class ImageController : Controller, IImageController
{
    private readonly IConfiguration _config;

    public ImageController(IConfiguration config)
    {
        _config = config;
    }

    public IActionResult Index()
    {
        throw new NotImplementedException();
    }

    [Route("/Image/{imageName}")]
    [HttpGet]
    public IActionResult Get(string imageName)
    {
        // Get the image from _config["MediaPath"] + imageName
        // Return the image

        var mediaPath = _config["MediaPath"];
        // combine the media path with the image name
        var imagePath = Path.Combine(mediaPath, imageName);
        var image = System.IO.File.OpenRead(imagePath);
        // check if image is null
        if (image == null)
            return NotFound();
        return File(image, "image/jpeg");
    }

    public async Task<IActionResult> Upload(List<IFormFile> files)
    {
        var mediaPath = _config["MediaPath"];
        // ensure the media path exists
        if (!Directory.Exists(mediaPath))
        {
            Directory.CreateDirectory(mediaPath);
        }

        // make sure we have files
        if (files == null || files.Count == 0)
        {
            return BadRequest();
        }

        var file = files[0];
        // ensure that the file is an image
        if (!file.ContentType.StartsWith("image/"))
        {
            return BadRequest();
        }

        // get new randomly generated file name
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        // while exists, generate new name
        while (System.IO.File.Exists(Path.Combine(mediaPath, fileName)))
        {
            fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        }

        if (mediaPath == null)
        {
            throw new FileNotFoundException("Media path not found - did you forget to set it?");
        }

        // save the file
        var fullPath = Path.Combine(mediaPath, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await files[0].CopyToAsync(fileStream);
        }

        // return new url for the image
        var serverName = Request.Host.Value;
        return Ok(new { location = $"/Image/{fileName}" });
    }
}
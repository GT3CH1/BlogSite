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
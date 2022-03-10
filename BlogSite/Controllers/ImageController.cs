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
        var currdir = Environment.CurrentDirectory;
        var path = Path.Combine($"{currdir}\\Media", imageName);
        byte[] image = {0};
        if(System.IO.File.Exists(path))
            image = System.IO.File.ReadAllBytes(path);
        return File(image, "image/jpeg");
    }


    [Route("/Image/UploadImage")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public string UploadImage(string fileData, string fileName)
    {
        var photoService = new PhotoService();
        string filePath;
        var res = photoService.Upload(fileData, fileName, out filePath);
        if (res)
        {
            return JsonConvert.SerializeObject(new ImageLocation
            {
                location = filePath,
            });
        }
        else
        {
            return JsonConvert.SerializeObject(new ImageLocation
            {
                location = "",
            });
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public class ImageController : Controller, IImageController
{
    public IActionResult Index()
    {
        throw new NotImplementedException();
    }

    [Route("/Image/Get/{imageName}")]
    [HttpGet]
    public string GetImage(string imageName)
    {
        //TODO: fetch file
        return "To be determined.";
    }
}
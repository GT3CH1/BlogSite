using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public interface IImageController
{
    public IActionResult Index();
    public IActionResult GetImage(string imageName);
}
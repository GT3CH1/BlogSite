using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public interface IImageController
{
    public IActionResult Upload();
}
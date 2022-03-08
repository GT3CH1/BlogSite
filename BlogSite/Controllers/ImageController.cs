using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public class ImageController
{
    [Authorize(Roles = "Admin")]
    public IActionResult Upload()
    {
        throw new NotImplementedException();
    }
}
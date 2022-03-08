using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public interface IHomeController
{
    public IActionResult Index();
    public IActionResult Privacy();
    public IActionResult Error();
}
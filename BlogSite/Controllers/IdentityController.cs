using BlogSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public class IdentityController : Controller
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
}
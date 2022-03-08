using System.Buffers.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace BlogSite.Controllers;

public class PostsController : Controller, IPostController
{
    // GET
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult CreatePost()
    {
        return View();
    }

    [Route("/Posts/View/{postId}")]
    public IActionResult PostsView(int postId)
    {
        PostModel post = new PostModel(postId);
        ViewBag.Message = post;
        return View();
    }

    [Route("/Posts/UploadImage")]
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public string UploadImage(string fileData, string fileName)
    {
        // FileObject fo = JsonConvert.DeserializeObject<FileObject>(fileObject);
        //TODO: Parse fileName and save fileData to somewhere secure.
        return JsonConvert.SerializeObject(new Location());
    }

    public class Location
    {
        [JsonProperty(PropertyName = "location")]
        public string location = "/change/me";
    }
}
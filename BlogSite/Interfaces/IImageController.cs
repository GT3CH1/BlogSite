using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers;

public interface IImageController
{
    /// <summary>
    /// Returns the index page of the image controller.
    /// </summary>
    /// <returns></returns>
    public IActionResult Index();

    /// <summary>
    /// Gets an image from the given file name.
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public IActionResult GetImage(string imageName);

    /// <summary>
    /// Uploads the image to the server.
    /// </summary>
    /// <param name="fileData">The base64 encoded version of the image.</param>
    /// <param name="fileName">The name of the image.</param>
    /// <returns></returns>
    public string UploadImage(string fileData, string fileName);
}
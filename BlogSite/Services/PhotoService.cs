/*
 * Copyright (c) 2022. Gavin Pease.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT
 * OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

namespace BlogSite.Services;

public class PhotoService : IPhotoService
{
    /// <summary>
    /// The media directory to write to - INCLUDING TRAILING SLASH
    /// </summary>
    public readonly string MEDIA_DIR = "./Media/";

    public readonly string[] ALLOWED_EXTENSIONS = { ".jpg", ".png", ".gif", ".webp" };

    /// <summary>
    /// Uploads a photo to the server.
    /// </summary>
    /// <param name="base64">The base64 encoding.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="filePath">The path of the file.</param>
    /// <returns></returns>
    public bool Upload(string base64, string fileName, out string filePath)
    {
        byte[] bytes = Convert.FromBase64String(base64);
        var extension = Path.GetExtension(fileName);

        if (!ALLOWED_EXTENSIONS.Contains(extension))
        {
            filePath = "";
            return false;
        }

        fileName = fileName.Replace(" ", "_");
        if (!Directory.Exists(MEDIA_DIR))
            Directory.CreateDirectory(MEDIA_DIR);
        filePath = Path.GetRelativePath(".", Path.Combine(MEDIA_DIR, GetNewImageName(fileName)));
        File.WriteAllBytes(filePath, bytes);
        filePath = $"/Image/Get/{Path.GetRelativePath("Media", filePath)}";
        return true;
    }

    /// <summary>
    /// Gets whether or not the image exists.
    /// </summary>
    /// <param name="filename">The filename</param>
    /// <returns>True if it exists, false otherwise.</returns>
    public bool ImageExists(string filename) => File.Exists(Path.Combine(MEDIA_DIR, filename));

    /// <summary>
    /// Gets the file path of the image if it exists.
    /// </summary>
    /// <param name="filename">The file name.</param>
    /// <returns>True if the path exists.</returns>
    public string GetPathIfImageExists(string filename)
    {
        if (ImageExists(filename))
            return $"/Image/Get/{Path.GetRelativePath("Media", filename)}";
        return "";
    }

    /// <summary>
    /// Generates a new image name if the image already exists.
    /// </summary>
    /// <param name="filename">The file name.</param>
    /// <returns>A new(?) file name</returns>
    public string GetNewImageName(string filename)
    {
        var extension = Path.GetExtension(filename);
        var name = Path.GetFileNameWithoutExtension(filename);
        var i = 1;
        while (ImageExists(filename))
        {
            filename = $"{name}_{i++}{extension}";
        }

        return filename;
    }

    /// <summary>
    /// Gets the image as base64 if it exists.
    /// </summary>
    /// <param name="imageName">The image name</param>
    /// <returns>The location of the image.</returns>
    public string GetImageAsBase64(string imageName)
    {
        var path = Path.Combine(MEDIA_DIR, imageName);
        if (!File.Exists(path))
            return "";
        return Convert.ToBase64String(File.ReadAllBytes(path));
    }
}
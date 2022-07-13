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

public interface IPhotoService
{
    private static Random random = new Random();

    /// <summary>
    /// Uploads the given byte array (image).
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
    public bool Upload(string fileName, string base64, out string path);

    /// <summary>
    /// Checks whether or not the given filename exists.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public bool ImageExists(string filename);

    /// <summary>
    /// Gets the filepath/name of an uploaded image.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetNewImageName(string filename);

    /// <summary>
    /// Gets a base64 image representation from the given imageName
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public string GetImageAsBase64(string imageName);

    public static string RandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
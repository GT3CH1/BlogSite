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
    /// Creates an alternative file name if there is a collision.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetAlternativeFilename(string filename);

    /// <summary>
    /// Gets the filepath/name of an uploaded image.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string GetNewImageName(string filename);

    public string GetImage(string imageName);

    public static string RandomString()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
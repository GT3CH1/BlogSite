namespace BlogSite.Services;

public class PhotoService : IPhotoService
{
    /// <summary>
    /// The media directory to write to - INCLUDING TRAILING SLASH
    /// </summary>
    public readonly string MEDIA_DIR = "./Media/";

    public readonly string[] ALLOWED_EXTENSIONS = { ".jpg", ".png", ".gif", ".webp" };

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
        filePath = Path.GetRelativePath(".",Path.Combine(MEDIA_DIR,GetNewImageName(fileName)));
        File.WriteAllBytes(filePath, bytes);
        filePath = $"/Image/Get/{Path.GetRelativePath("Media",filePath)}";
        return true;
    }

    public bool ImageExists(string filename)
    {
        var fileExists = File.Exists(filename);
        if (!fileExists)
            fileExists = File.Exists($"{MEDIA_DIR}{filename}");
        return fileExists;
    }

    public string GetPathIfImageExists(string filename)
    {
        var path = filename;
        if (!File.Exists(filename))
            path = $"{MEDIA_DIR}{filename}";
        if (!File.Exists(path))
            return "";
        return $"/Media/{filename}";
    }

    public string GetAlternativeFilename(string filename)
    {
        var fileName = Path.GetFileNameWithoutExtension($"{MEDIA_DIR}{filename}");
        var extension = Path.GetExtension(filename);
        return $"{fileName}_{IPhotoService.RandomString()}{extension}";
    }

    public string GetNewImageName(string filename)
    {
        if (ImageExists(filename))
        {
            var imageExists = ImageExists(GetAlternativeFilename(filename));
            while (imageExists)
                imageExists = ImageExists(GetAlternativeFilename(filename));
            return $"{GetAlternativeFilename(filename)}";
        }

        return $"{filename}";
    }

    public string GetImage(string imageName)
    {
        var img = File.ReadAllBytes($"{MEDIA_DIR}{imageName}");
        return Convert.ToBase64String(img);
    }
}
using Newtonsoft.Json;

namespace BlogSite.Models;

public class ImageLocation
{
    [JsonProperty(PropertyName = "location")]
    public string location = "/change/me";
}
using Newtonsoft.Json;

namespace BlogSite.Models;

public class ImageLocation
{
    [JsonProperty(PropertyName = "location")]
    public string ImagePath { get; set; } = "/change/me";
}
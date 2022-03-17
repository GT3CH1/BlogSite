namespace BlogSite.Interfaces;

public interface IPostModel
{
    /// <summary>
    /// The ID of a post.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// The title of a post.
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// The contents of a post.
    /// </summary>
    public string Content { get; set; }
}
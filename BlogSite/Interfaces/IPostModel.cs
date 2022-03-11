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
    
    /// <summary>
    /// Deletes this post from the database.
    /// </summary>
    public void Delete();
    
    /// <summary>
    /// Updates this post, by setting the post title and content.
    /// </summary>
    public void Update();
    
    /// <summary>
    /// Creates this post in the database.
    /// </summary>
    public int Create();
}
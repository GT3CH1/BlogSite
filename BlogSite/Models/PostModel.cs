using BlogSite.Data;

namespace BlogSite.Models;

public class PostModel
{
    public int Id { get; set; } = -1;
    public string Title { get; private set; }
    public string Content { get; private set; }

    public PostModel()
    {
        
    }

    public PostModel(string title, string content, int id)
    {
        Title = title;
        Content = content;
        Id = id;
    }

    public PostModel(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
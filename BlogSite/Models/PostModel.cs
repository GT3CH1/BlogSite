using BlogSite.Data;
using BlogSite.Interfaces;

namespace BlogSite.Models;

public class PostModel : IPostModel
{
    public int Id { get; set; } = -1;

    /// <summary>
    /// The title, without any script tags.
    /// </summary>
    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            _title = _title.Replace("script", "");
            _title = _title.Replace("onload", "");
            _title = _title.Replace("onerror", "");
        }
    }

    /// <summary>
    /// The content without any script tags.
    /// </summary>
    private string _content = string.Empty;

    public string Content
    {
        get => _content;
        set
        {
            _content = value;
            _content = _content.Replace("script", "");
            _title = _title.Replace("onload", "");
            _title = _title.Replace("onerror", "");
        }
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

    public void Delete() => PostDatabaseModel.DeletePost(this);
    public void Update() => PostDatabaseModel.EditPost(this);
    public void Create() => PostDatabaseModel.CreatePost(this);
}
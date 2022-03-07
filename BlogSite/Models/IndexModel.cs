using BlogSite.Data;

namespace BlogSite.Models;

public class IndexModel
{
    private BlogPostController _controller;

    public IndexModel()
    {
        _controller = new BlogPostController();
    }

    public BlogPostController GetPostController() => _controller;
}
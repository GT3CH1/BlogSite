using System.Diagnostics;
using System.Text.RegularExpressions;
using BlogSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace BlogSite.Data;

public class BlogPostController : Controller, IBlogPostController
{
    private static string connectionString = "Data Source=web.peasenet.com;" +
                                             "User ID=BlogBot;" +
                                             "Password=yg61ueI7Py46tekp;" +
                                             "database=Blog";

    /// <summary>
    /// Gets the total number of posts in existence.
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfPosts()
    {
        if (Debugger.IsAttached)
            return 1;
        int numberOfPosts = 0;
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) count FROM Posts;";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    numberOfPosts = int.Parse(reader["count"].ToString());
            }
        }

        return numberOfPosts;
    }

    /// <summary>
    /// Gets the content of a post.
    /// </summary>
    /// <param name="postId">The post id to get the content of.</param>
    /// <returns>The content in a post.</returns>
    public string GetContentOfPost(int postId)
    {
        if (Debugger.IsAttached)
            return "<h1>This is a debugged HTML tag in <i>italics</i></h1>";
        return GetDataFromRow("Content", postId);
    }

    public List<int> GetListOfPostIds()
    {
        List<int> postIds = new List<int>();
        if (Debugger.IsAttached)
        {
            postIds.Add(0);
            return postIds;
        }

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT ID FROM Posts;";
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    postIds.Add(int.Parse(reader["ID"].ToString()));
        }
        return postIds;
    }

    /// <summary>
    /// Gets the title of a post
    /// </summary>
    /// <param name="postId">The post ID</param>
    /// <returns>The title of a post.</returns>
    public string GetTitleOfPost(int postId)
    {
        if (Debugger.IsAttached)
            return "This is a title.";
        return GetDataFromRow("Title", postId);
    }
    
    public string StripHtmlTags(string html)
    {
        var noHtmlTags = Regex.Replace(html, "<.*?>", string.Empty);
        return Regex.Replace(noHtmlTags,"&.*?;",string.Empty);
    }
    
    /// <summary>
    /// Gets data from the given row
    /// </summary>
    /// <param name="row"></param>
    /// <param name="postId"></param>
    /// <returns></returns>
    private string GetDataFromRow(string row, int postId)
    {
        var data = string.Empty;
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT {row} FROM Posts WHERE ID = {postId}";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    data = reader[$"{row}"].ToString();
                }
            }
        }

        return data;
    }

}
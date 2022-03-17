using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Transactions;
using BlogSite.Data;

namespace BlogSite.Models;

/// <summary>
/// A model representing a connection to the post database.  This model can modify the database.
/// </summary>
public class PostDatabaseController
{
    /// <summary>
    /// The connection string to use for connecting to the post database.
    /// Default is "DataSource=posts.db;Cache=Shared"
    /// </summary>
    public static string ConnectionString { get; set; } = "DataSource=posts.db;Cache=Shared";

    public static ApplicationDbContext Context { get; set; }

    /// <summary>
    /// Adds the given post to the database.
    /// </summary>
    /// <param name="post">The post to add.</param>
    /// <exception cref="ArgumentException">Thrown when the title or contents of a post are empty.</exception>
    public static int CreatePost(PostModel post)
    {
        int newId = -1;
        if (post.Content == string.Empty)
            throw new ArgumentException("The contents of a post cannot be empty!");
        if (post.Title == string.Empty)
            throw new ArgumentException("The title of a post cannot be empty!");

        using (var scope = new TransactionScope())
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Posts (Title,Content) VALUES(@title,@content);" +
                                  "SELECT last_insert_rowid();";
                // postTitle = postTitle.Replace(""","\\\"");
                // postContent = postContent.Replace("\"","\\\"");
                cmd.Parameters.AddWithValue("@title", post.Title);
                cmd.Parameters.AddWithValue("@content", post.Content);
                cmd.Prepare();
                newId = Convert.ToInt32(cmd.ExecuteScalar());
                Save();
            }

            scope.Complete();
        }

        return newId;
    }

    /// <summary>
    /// Sets the content and title of a post with the post id.
    /// </summary>
    /// <param name="post">The post containing the updated content/title</param>
    /// <exception cref="ArgumentException">thrown when title, content, or id is not set.</exception>
    public static void EditPost(PostModel post)
    {
        if (post.Title == string.Empty)
            throw new ArgumentException("The title cannot be empty.");
        if (post.Content == string.Empty)
            throw new ArgumentException("The content cannot be empty.");
        if (post.Id == -1)
            throw new ArgumentException("The post id cannot be invalid.");

        using (var scope = new TransactionScope())
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = "UPDATE Posts SET Title=@title, Content=@content WHERE ID=@id";
                var cmd = connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.Parameters.AddWithValue("@title", post.Title);
                cmd.Parameters.AddWithValue("@content", post.Content);
                cmd.Parameters.AddWithValue("@id", post.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Save();
            }

            scope.Complete();
        }
    }

    /// <summary>
    /// Deletes a post containing the same id.
    /// </summary>
    /// <param name="post">The post to delete.</param>
    /// <exception cref="ArgumentException">Thrown when the Id is not set.</exception>
    public static void DeletePost(PostModel post)
    {
        if (post.Id == -1)
            throw new ArgumentException("The post id has to be set.");
        using (var scope = new TransactionScope())
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                var cmdText = "DELETE FROM Posts WHERE ID=@id";
                var cmd = connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.Parameters.AddWithValue("@id", post.Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Save();
            }

            scope.Complete();
        }
    }

    /// <summary>
    /// Gets a post from the given id.
    /// </summary>
    /// <param name="id">The ID of the post to get data from.</param>
    /// <returns>A PostModel containing the content, title, and id.</returns>
    public static PostModel GetPostById(int id)
    {
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var cmdText = "SELECT * FROM Posts WHERE ID=@id";
            var cmd = connection.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@id", id);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int postId = -1;
                    if (reader["Title"] == null || reader["Content"] == null || reader["ID"] == null)
                        throw new InvalidOperationException("Title, content, or ID is null.");
                    var postTitle = reader["Title"].ToString();
                    var postContent = reader["Content"].ToString();
                    var output = reader["ID"].ToString();
                    int.TryParse(output, out postId);
                    if (postTitle != null && postContent != null && postId > 0)
                        return new PostModel(postTitle, postContent, postId);
                    throw new InvalidOperationException("Found invalid post!");
                }
            }
        }

        throw new ArgumentException("Invalid post id");
    }

    /// <summary>
    /// Gets the number of posts 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static int GetNumberOfPosts() => GetPostIdList().Count;

    /// <summary>
    /// Gets a list of all available Ids of posts.
    /// </summary>
    /// <returns>A list of integers representing post ids.</returns>
    public static List<int> GetPostIdList()
    {
        var postIds = new List<int>();
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();
            var cmdText = "SELECT ID FROM Posts;";
            var cmd = connection.CreateCommand();
            cmd.CommandText = cmdText;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int postId;
                    if (!reader.HasRows)
                        continue;
                    var output = reader["ID"].ToString();
                    int.TryParse(output, out postId);
                    postIds.Add(postId);
                }
            }
        }

        return postIds;
    }

    /// <summary>
    /// Strips the html tags of the given string.
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static string StripHtmlTags(string html)
    {
        var noHtmlTags = Regex.Replace(html, "<.*?>", string.Empty);
        return Regex.Replace(noHtmlTags, "&.*?;", string.Empty);
    }

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <param name="context"></param>
    private static void Save()
    {
        Context.SaveChanges();
    }
}
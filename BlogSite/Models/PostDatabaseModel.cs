using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Transactions;

namespace BlogSite.Models;

/// <summary>
/// A model representing a connection to the post database.  This model can modify the database.
/// </summary>
public class PostDatabaseModel
{
    /// <summary>
    /// The connection string to use for connecting to the post database.
    /// </summary>
    public static string ConnectionString { get; private set; }

    /// <summary>
    /// The file path of the post database.
    /// </summary>
    public static string FilePath { get; private set; }

    /// <summary>
    /// The string used to create the post table.
    /// </summary>
    private static readonly string PostsTableCommand = "CREATE TABLE IF NOT EXISTS " +
                                                       "Posts (Title TEXT NOT NULL, Content TEXT NOT NULL, " +
                                                       "ID INTEGER AUTOINCREMENT DEFAULT 0, PRIMARY KEY (ID))";

    /// <summary>
    /// Creates a new Database model for containing posts. If the filePath does not exist,
    /// an attempt to make a new database.
    /// </summary>
    /// <param name="connectionString">The connection string to use</param>
    /// <param name="filePath">The path of the database.</param>
    public PostDatabaseModel(string connectionString, string filePath)
    {
        ConnectionString = connectionString;
        FilePath = filePath;

        CreateDatabase();
    }

    /// <summary>
    /// Creates a new post database connection.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when ConnectionString or FilePath are empty.</exception>
    public PostDatabaseModel()
    {
        if (ConnectionString == string.Empty) throw new ArgumentException("ConnectionString cannot be empty");
        if (FilePath == string.Empty) throw new ArgumentException("FilePath cannot be empty.");
    }

    /// <summary>
    /// Creates an empty post database.
    /// </summary>
    public static void CreateDatabase()
    {
        using (TransactionScope scope = new TransactionScope())
        {
            if (!File.Exists(FilePath))
                SQLiteConnection.CreateFile(FilePath);
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(PostsTableCommand, conn);
                cmd.ExecuteNonQuery();
            }

            scope.Complete();
        }
    }

    /// <summary>
    /// Adds the given post to the database.
    /// </summary>
    /// <param name="post">The post to add.</param>
    /// <exception cref="ArgumentException">Thrown when the title or contents of a post are empty.</exception>
    public static void CreatePost(PostModel post)
    {
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
                cmd.CommandText = "INSERT INTO Posts (Title,Content) VALUES(@title,@content);";
                // postTitle = postTitle.Replace(""","\\\"");
                // postContent = postContent.Replace("\"","\\\"");
                cmd.Parameters.AddWithValue("@title", post.Title);
                cmd.Parameters.AddWithValue("@content", post.Content);
                cmd.ExecuteNonQuery();
            }

            scope.Complete();
        }
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
                cmd.ExecuteNonQuery();
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
                cmd.ExecuteNonQuery();
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
            cmd.Parameters.AddWithValue("@id", id);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var postTitle = reader["Title"].ToString();
                    var postContent = reader["Content"].ToString();
                    var postId = int.Parse(reader["ID"].ToString());
                    return new PostModel(postTitle, postContent, postId);
                }
            }
        }

        return new PostModel();
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
                    if (!reader.HasRows)
                        continue;
                    var output = reader["ID"].ToString();
                    int postId = int.Parse(reader["ID"].ToString());
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
}
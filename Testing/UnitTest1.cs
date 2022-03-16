using System;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using BlogSite;
using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Testing;

[SetUpFixture]
public class Setup
{
    public static ApplicationDbContext context;
    public SQLiteConnection connection;
    public WebApplicationBuilder builder;
    public readonly string ConnectionString = "DataSource=testing.db;Cache=Shared";

    [OneTimeSetUp]
    public void SetUp()
    {
        if (File.Exists("testing.db"))
            File.Delete("testing.db");
        builder = WebApplication.CreateBuilder();
        Startup.SetupDatabase(builder, ConnectionString);
        var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;
        context = new ApplicationDbContext(contextOptions);
        context.Database.EnsureCreated();
        context.Database.Migrate();

        PostDatabaseModel.ConnectionString = ConnectionString;
    }
}

[TestFixture]
public class Tests
{
    private static int createdPostId;

    [Test]
    public async Task CreateAdminRole()
    {
        Assert.False(await Startup.AdminRoleExists(Startup.GetRoleManager(Setup.context)));
        Startup.CreateAdminRoles(Startup.GetRoleManager(Setup.context));
        Assert.True(await Startup.AdminRoleExists(Startup.GetRoleManager(Setup.context)));
    }


    [Test]
    public void TestGetInvalidPostId()
    {
        Assert.Throws<ArgumentException>(() => { PostDatabaseModel.GetPostById(-1); });
    }

    [Test]
    public void CreatePost()
    {
        PostModel p = new PostModel("Test", "<h1>test content</h1>");
        var postId = PostDatabaseModel.CreatePost(p);
        var postFromDb = PostDatabaseModel.GetPostById(postId);
        Assert.AreEqual(postFromDb.Content, p.Content);
        Assert.AreEqual(postFromDb.Title, p.Title);
        Assert.AreEqual(postFromDb.Id, postId);
        // Ensure the post is deleted.
        p.Id = postId;
        PostDatabaseModel.DeletePost(postFromDb);
    }

    [Test]
    public void DeletePost()
    {
        PostModel p = new PostModel("Test", "<h1>test content</h1>");
        var postId = PostDatabaseModel.CreatePost(p);
        var postFromDb = PostDatabaseModel.GetPostById(postId);
        Assert.AreEqual(postFromDb.Content, p.Content);
        Assert.AreEqual(postFromDb.Title, p.Title);
        Assert.AreEqual(postFromDb.Id, postId);
        // Ensure the post is deleted.
        PostDatabaseModel.DeletePost(postFromDb);
        Assert.Throws<ArgumentException>(() => { PostDatabaseModel.GetPostById(postId); });
    }

    [Test]
    public void EditPost()
    {
        var oldContent = "<h1>test content</h1>";
        var newContent = "<h2>test content</h2>";
        PostModel p = new PostModel("Test", oldContent);
        var postId = PostDatabaseModel.CreatePost(p);
        p = PostDatabaseModel.GetPostById(postId);
        p.Content = newContent;
        PostDatabaseModel.EditPost(p);
        p = PostDatabaseModel.GetPostById(postId);
        Assert.AreNotEqual(oldContent, p.Content);
        PostDatabaseModel.DeletePost(p);
    }

    [Test]
    public void CreatePostEmptyTitle()
    {
        var p = new PostModel();
        p.Content = "test";
        Assert.Throws<ArgumentException>(() => PostDatabaseModel.CreatePost(p));
    }

    [Test]
    public void CreatePostEmptyContent()
    {
        var p = new PostModel();
        p.Title = "test";
        Assert.Throws<ArgumentException>(() => PostDatabaseModel.CreatePost(p));
    }

    [Test]
    public void EditPostEmptyTitle()
    {
        var p = new PostModel("", "Content", 1);
        Assert.Throws<ArgumentException>(() => PostDatabaseModel.EditPost(p));
    }

    [Test]
    public void EditPostEmptyContent()
    {
        var p = new PostModel("title", "",1);
        Assert.Throws<ArgumentException>(() => PostDatabaseModel.EditPost(p));
    }

    [Test]
    public void EditPostBadId()
    {
        var p = new PostModel("title", "123", -1);
        Assert.Throws<ArgumentException>(() => PostDatabaseModel.EditPost(p));
    }

    [Test]
    public void DeletePostBadId()
    {
        var p = new PostModel("title", "123", -1);
        Assert.Throws<ArgumentException>(() => PostDatabaseModel.DeletePost(p));
    }

    [Test]
    public void PostIdListEmptyAtStart()
    {
        Assert.AreEqual(0,PostDatabaseModel.GetNumberOfPosts());
    }

    [Test]
    public void AddingPostChangesIdList()
    {
        var countBefore = PostDatabaseModel.GetPostIdList().Count;
        var post = new PostModel("123", "123");
        var postId = PostDatabaseModel.CreatePost(post);
        Assert.AreEqual(countBefore + 1, PostDatabaseModel.GetNumberOfPosts());
        post.Id = postId;
        PostDatabaseModel.DeletePost(post);
    }

    [Test]
    public void PostIdListContainsNewPostId()
    {
        var post = new PostModel("123", "123");
        var postId = PostDatabaseModel.CreatePost(post);
        Assert.True(PostDatabaseModel.GetPostIdList().Contains(postId));
        post.Id = postId;
        PostDatabaseModel.DeletePost(post);
    }
}
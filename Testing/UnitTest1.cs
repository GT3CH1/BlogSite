using System.Threading.Tasks;
using BlogSite;
using BlogSite.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Testing;

public class Tests
{
    private ApplicationDbContext context;
    [OneTimeSetUp]
    public void SetUp()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;
        context = new ApplicationDbContext(contextOptions);
        context.Database.EnsureCreated();  
    }
    [Test]
    public async Task CreateAdminRole()
    {
        Assert.False(await Startup.AdminRoleExists( Startup.GetRoleManager(context)));
        Startup.CreateAdminRoles(Startup.GetRoleManager(context));
        Assert.True(await Startup.AdminRoleExists( Startup.GetRoleManager(context)));
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //if(Database.EnsureCreated())
        try
        {
            Database.Migrate();
        }
        catch
        {
            Console.Error.WriteLine("Failed to migrate database");
        }
    }
}
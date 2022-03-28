using BlogSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace BlogSite;

public class Startup
{
    /// <summary>
    /// Creates the admin role.
    /// </summary>
    /// <param name="store"></param>
    public static async void CreateAdminRoles(RoleManager<IdentityRole> store)
    {
        if (!await AdminRoleExists(store))
            await store.CreateAsync(new IdentityRole { Name = "Admin" });
    }

    public static async Task<bool> AdminRoleExists(RoleManager<IdentityRole> store)
    {
        return await store.FindByNameAsync("Admin") != null;
    }

    /// <summary>
    /// Adds a list of administrators to the admin roles.
    /// </summary>
    /// <param name="configurationSections">The configuration containing the list of administrators.</param>
    /// <param name="userManager"></param>
    public static async Task AddAdminsToAdminRole(List<string> adminUsers, UserManager<IdentityUser> userManager)
    {
        foreach (var adminUser in adminUsers)
        {
            var user = await userManager.FindByEmailAsync(adminUser.ToUpper());
            if (user != null)
            {
                var currRoles = await userManager.GetRolesAsync(user);
                if (!currRoles.Contains("Admin"))
                    await userManager.AddToRoleAsync(user, "Admin");
                await userManager.UpdateAsync(user);
            }
        }
    }

    /// <summary>
    /// Creates a new user manager from the application database context
    /// </summary>
    /// <param name="applicationDbContext"></param>
    /// <returns></returns>
    public static UserManager<IdentityUser> GetUserManager(ApplicationDbContext applicationDbContext)
    {
        var userStore = new UserStore<IdentityUser>(applicationDbContext);
        var userManager1 = new UserManager<IdentityUser>(userStore, null, null, null, null,
            new UpperInvariantLookupNormalizer(), null, null,
            new NullLogger<UserManager<IdentityUser>>());
        return userManager1;
    }

    /// <summary>
    /// Creates a role manager.
    /// </summary>
    /// <param name="applicationDbContext"></param>
    /// <returns></returns>
    public static RoleManager<IdentityRole> GetRoleManager(ApplicationDbContext applicationDbContext)
    {
        var roleStore = new RoleStore<IdentityRole>(applicationDbContext); //Pass the instance of your DbContext here
        var roleManager1 =
            new RoleManager<IdentityRole>(roleStore, null, new UpperInvariantLookupNormalizer(), null, null);
        return roleManager1;
    }

    public static void SetupDatabase(WebApplicationBuilder webApplicationBuilder, string appConnectionString,
        string postConnectionString)
    {
        webApplicationBuilder.Services
            .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(appConnectionString));
        webApplicationBuilder.Services.AddDbContext<PostDbContext>(options =>
            options.UseSqlite(postConnectionString));
    }
}
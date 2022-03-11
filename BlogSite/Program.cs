using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogSite.Data;
using BlogSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

var builder = WebApplication.CreateBuilder(args);
ILogger _logger = NullLogger.Instance;
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
new PostDatabaseModel(connectionString, "app.db");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
});
builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddLogging();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleStore = new RoleStore<IdentityRole>(dbContext); //Pass the instance of your DbContext here
    var userStore = new UserStore<IdentityUser>(dbContext);
    var roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null);
    if (roleManager.FindByNameAsync("Admin") == null)
        await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
    var userManager = new UserManager<IdentityUser>(userStore, null, null, null, null, null, null, null,
        NullLogger<UserManager<IdentityUser>>.Instance);
    var user = await userManager.FindByNameAsync("changeme@changeme.com".ToUpper());
    if (user != null)
    {
        var currRoles = userManager.GetRolesAsync(user).Result;

        if (!currRoles.Contains("Admin"))
            await userManager.AddToRoleAsync(user, "Admin");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
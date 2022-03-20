using BlogSite;
using BlogSite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

var builder = WebApplication.CreateBuilder(args);
ILogger logger = NullLogger.Instance;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Startup.SetupDatabase(builder, connectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
});
builder.Services.AddControllersWithViews();

builder.Services.AddLogging();
var app = builder.Build();
// PostDatabaseController.Context = app.Services.GetService<ApplicationDbContext>();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    Startup.CreateAdminRoles(Startup.GetRoleManager(dbContext));
    var userList = builder.Configuration.GetSection("Administrators").GetChildren();
    var adminUsers = new List<string>();
    foreach (var user in userList)
        adminUsers.Add(user.Value);
    await Startup.AddAdminsToAdminRole(adminUsers, Startup.GetUserManager(dbContext));
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
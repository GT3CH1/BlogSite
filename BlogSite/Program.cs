/*
 * Copyright (c) 2022. Gavin Pease.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT
 * OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

using BlogSite;
using BlogSite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

var builder = WebApplication.CreateBuilder(args);
ILogger logger = NullLogger.Instance;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var postConnectionString = builder.Configuration.GetConnectionString("PostDatabase");
Startup.SetupDatabase(builder, connectionString, postConnectionString);
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
    var postDb = scope.ServiceProvider.GetService<PostDbContext>();
    postDb.Database.Migrate();
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
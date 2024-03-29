﻿/*
 * Copyright (c) 2022. Gavin Pease and contributors.
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

using BlogSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Data;

public class ApplicationDbContext : IdentityDbContext<Poster>
{
    public DbSet<BlogSite.Models.Posts> Posts { get; set; }

    public async Task InitializeUsers(UserManager<Poster> um, RoleManager<IdentityRole> rm)
    {
        var adminRole = new IdentityRole("Admin");

        if (await um.Users.AnyAsync())
            return;
        // seed the db. 

        var admin = new Poster
        {
            UserName = "admin@admin.com",
            Email = "admin@admin.com",
            Name = "Administrator",
            EmailConfirmed = true,
        };

        await um.CreateAsync(admin, "Admin123!");
        await rm.CreateAsync(adminRole);
        await um.AddToRoleAsync(admin, "Admin");
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public async Task InitializeDatabase()
    {
        if ((await Database.GetPendingMigrationsAsync()).Any())
            await Database.MigrateAsync();
    }
}
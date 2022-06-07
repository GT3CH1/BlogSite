using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSite.Migrations.PostDb
{
    public partial class AddDrafts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add draft boolean to posts table.
            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Posts",
                nullable: false,
                defaultValue: false);
        }
    }
}

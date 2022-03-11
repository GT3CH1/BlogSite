using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogSite.Data.Migrations
{
    public partial class AddPostTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Title = table.Column<string>(nullable: false)
                        .Annotation("Sqlite:AutoIncrement", true),
                    Content = table.Column<string>(nullable: false),
                    ID = table.Column<int>(nullable: false),
                },
                constraints: table => {table.PrimaryKey("ID", x => x.ID);}
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Posts");
        }
    }
}

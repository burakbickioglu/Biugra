using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biugra.Domain.Migrations
{
    public partial class ForumImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Forums",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Forums");
        }
    }
}

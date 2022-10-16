using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeShop.Migrations
{
    public partial class AddImageUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURl",
                table: "Users");
        }
    }
}

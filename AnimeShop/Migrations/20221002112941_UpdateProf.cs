using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeShop.Migrations
{
    public partial class UpdateProf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "ProductsList");

            migrationBuilder.RenameColumn(
                name: "NewPrice",
                table: "ProductsList",
                newName: "Discount");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductsList",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductsList");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "ProductsList",
                newName: "NewPrice");

            migrationBuilder.AddColumn<int>(
                name: "OldPrice",
                table: "ProductsList",
                type: "int",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    public partial class AddCityToCinemaLineTheaters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CinemaLine",
                table: "Theaters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Theaters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CinemaLine",
                table: "Theaters");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Theaters");

        }
    }
}

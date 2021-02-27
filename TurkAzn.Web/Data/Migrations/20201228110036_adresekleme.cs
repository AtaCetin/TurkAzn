using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class adresekleme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdresName",
                table: "Adres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "il",
                table: "Adres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ilce",
                table: "Adres",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdresName",
                table: "Adres");

            migrationBuilder.DropColumn(
                name: "il",
                table: "Adres");

            migrationBuilder.DropColumn(
                name: "ilce",
                table: "Adres");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class marketek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isbaslik",
                table: "Marketler",
                newName: "legalCompanyTitle");

            migrationBuilder.AddColumn<string>(
                name: "MarketTipi",
                table: "Marketler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParaTipi",
                table: "Marketler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VergiDairesi",
                table: "Marketler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VergiNumarasi",
                table: "Marketler",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketTipi",
                table: "Marketler");

            migrationBuilder.DropColumn(
                name: "ParaTipi",
                table: "Marketler");

            migrationBuilder.DropColumn(
                name: "VergiDairesi",
                table: "Marketler");

            migrationBuilder.DropColumn(
                name: "VergiNumarasi",
                table: "Marketler");

            migrationBuilder.RenameColumn(
                name: "legalCompanyTitle",
                table: "Marketler",
                newName: "isbaslik");
        }
    }
}

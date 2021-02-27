using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class spetid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SepetID",
                table: "Satislar",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SepetID",
                table: "Satislar");
        }
    }
}

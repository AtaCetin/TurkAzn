using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class adressatis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "istek",
                table: "Satislar");

            migrationBuilder.AddColumn<int>(
                name: "verilenAdresadresID",
                table: "Satislar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Satislar_verilenAdresadresID",
                table: "Satislar",
                column: "verilenAdresadresID");

            migrationBuilder.AddForeignKey(
                name: "FK_Satislar_Adresler_verilenAdresadresID",
                table: "Satislar",
                column: "verilenAdresadresID",
                principalTable: "Adresler",
                principalColumn: "adresID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Satislar_Adresler_verilenAdresadresID",
                table: "Satislar");

            migrationBuilder.DropIndex(
                name: "IX_Satislar_verilenAdresadresID",
                table: "Satislar");

            migrationBuilder.DropColumn(
                name: "verilenAdresadresID",
                table: "Satislar");

            migrationBuilder.AddColumn<string>(
                name: "istek",
                table: "Satislar",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

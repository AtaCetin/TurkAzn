using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class adresler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adres_AspNetUsers_KullaniciId",
                table: "Adres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adres",
                table: "Adres");

            migrationBuilder.DropColumn(
                name: "il",
                table: "Adres");

            migrationBuilder.RenameTable(
                name: "Adres",
                newName: "Adresler");

            migrationBuilder.RenameIndex(
                name: "IX_Adres_KullaniciId",
                table: "Adresler",
                newName: "IX_Adresler_KullaniciId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adresler",
                table: "Adresler",
                column: "adresID");

            migrationBuilder.AddForeignKey(
                name: "FK_Adresler_AspNetUsers_KullaniciId",
                table: "Adresler",
                column: "KullaniciId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adresler_AspNetUsers_KullaniciId",
                table: "Adresler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adresler",
                table: "Adresler");

            migrationBuilder.RenameTable(
                name: "Adresler",
                newName: "Adres");

            migrationBuilder.RenameIndex(
                name: "IX_Adresler_KullaniciId",
                table: "Adres",
                newName: "IX_Adres_KullaniciId");

            migrationBuilder.AddColumn<string>(
                name: "il",
                table: "Adres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adres",
                table: "Adres",
                column: "adresID");

            migrationBuilder.AddForeignKey(
                name: "FK_Adres_AspNetUsers_KullaniciId",
                table: "Adres",
                column: "KullaniciId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

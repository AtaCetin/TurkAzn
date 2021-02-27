using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class marketekleme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_AspNetUsers_UrunSahibiId",
                table: "Urunler");

            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Marketler_MarketID",
                table: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Urunler_MarketID",
                table: "Urunler");

            migrationBuilder.DropColumn(
                name: "MarketID",
                table: "Urunler");

            migrationBuilder.RenameColumn(
                name: "UrunSahibiId",
                table: "Urunler",
                newName: "UrunSahibiMarketID");

            migrationBuilder.RenameIndex(
                name: "IX_Urunler_UrunSahibiId",
                table: "Urunler",
                newName: "IX_Urunler_UrunSahibiMarketID");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Marketler_UrunSahibiMarketID",
                table: "Urunler",
                column: "UrunSahibiMarketID",
                principalTable: "Marketler",
                principalColumn: "MarketID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urunler_Marketler_UrunSahibiMarketID",
                table: "Urunler");

            migrationBuilder.RenameColumn(
                name: "UrunSahibiMarketID",
                table: "Urunler",
                newName: "UrunSahibiId");

            migrationBuilder.RenameIndex(
                name: "IX_Urunler_UrunSahibiMarketID",
                table: "Urunler",
                newName: "IX_Urunler_UrunSahibiId");

            migrationBuilder.AddColumn<string>(
                name: "MarketID",
                table: "Urunler",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_MarketID",
                table: "Urunler",
                column: "MarketID");

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_AspNetUsers_UrunSahibiId",
                table: "Urunler",
                column: "UrunSahibiId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Urunler_Marketler_MarketID",
                table: "Urunler",
                column: "MarketID",
                principalTable: "Marketler",
                principalColumn: "MarketID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class kargo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KargoNum",
                table: "Satislar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "KargoYollandimi",
                table: "Satislar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "YollananKargoSirketiKargoSirketiID",
                table: "Satislar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KargoSirketi",
                columns: table => new
                {
                    KargoSirketiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kontrollinki = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KargoSirketi", x => x.KargoSirketiID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Satislar_YollananKargoSirketiKargoSirketiID",
                table: "Satislar",
                column: "YollananKargoSirketiKargoSirketiID");

            migrationBuilder.AddForeignKey(
                name: "FK_Satislar_KargoSirketi_YollananKargoSirketiKargoSirketiID",
                table: "Satislar",
                column: "YollananKargoSirketiKargoSirketiID",
                principalTable: "KargoSirketi",
                principalColumn: "KargoSirketiID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Satislar_KargoSirketi_YollananKargoSirketiKargoSirketiID",
                table: "Satislar");

            migrationBuilder.DropTable(
                name: "KargoSirketi");

            migrationBuilder.DropIndex(
                name: "IX_Satislar_YollananKargoSirketiKargoSirketiID",
                table: "Satislar");

            migrationBuilder.DropColumn(
                name: "KargoNum",
                table: "Satislar");

            migrationBuilder.DropColumn(
                name: "KargoYollandimi",
                table: "Satislar");

            migrationBuilder.DropColumn(
                name: "YollananKargoSirketiKargoSirketiID",
                table: "Satislar");
        }
    }
}

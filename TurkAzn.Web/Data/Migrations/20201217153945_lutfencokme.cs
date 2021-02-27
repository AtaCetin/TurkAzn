using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class lutfencokme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KategoriID",
                table: "Marketler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LogoResimID",
                table: "Marketler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reklam",
                columns: table => new
                {
                    ReklamID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReklamAciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReklamResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KucukReklamResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KategoriID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reklam", x => x.ReklamID);
                    table.ForeignKey(
                        name: "FK_Reklam_Kategoriler_KategoriID",
                        column: x => x.KategoriID,
                        principalTable: "Kategoriler",
                        principalColumn: "KategoriID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Marketler_KategoriID",
                table: "Marketler",
                column: "KategoriID");

            migrationBuilder.CreateIndex(
                name: "IX_Marketler_LogoResimID",
                table: "Marketler",
                column: "LogoResimID");

            migrationBuilder.CreateIndex(
                name: "IX_Reklam_KategoriID",
                table: "Reklam",
                column: "KategoriID");

            migrationBuilder.AddForeignKey(
                name: "FK_Marketler_Kategoriler_KategoriID",
                table: "Marketler",
                column: "KategoriID",
                principalTable: "Kategoriler",
                principalColumn: "KategoriID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Marketler_Resimler_LogoResimID",
                table: "Marketler",
                column: "LogoResimID",
                principalTable: "Resimler",
                principalColumn: "ResimID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marketler_Kategoriler_KategoriID",
                table: "Marketler");

            migrationBuilder.DropForeignKey(
                name: "FK_Marketler_Resimler_LogoResimID",
                table: "Marketler");

            migrationBuilder.DropTable(
                name: "Reklam");

            migrationBuilder.DropIndex(
                name: "IX_Marketler_KategoriID",
                table: "Marketler");

            migrationBuilder.DropIndex(
                name: "IX_Marketler_LogoResimID",
                table: "Marketler");

            migrationBuilder.DropColumn(
                name: "KategoriID",
                table: "Marketler");

            migrationBuilder.DropColumn(
                name: "LogoResimID",
                table: "Marketler");
        }
    }
}

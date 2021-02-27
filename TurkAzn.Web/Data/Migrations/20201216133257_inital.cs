using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TurkAzn.Web.Data.Migrations
{
    public partial class inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AktifMi",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AktifOlmamaNedeni",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvatarResimID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DegistirilmeTarihi",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnSonOturumAcma",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MarketHesabimi",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketProfiliMarketID",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OlusturulmaTarihi",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Soyad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon_Numarasi",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rol_AD",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bildirim",
                columns: table => new
                {
                    BildirimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BildirimIcerik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BildirimYollayan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bildirimLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    okunduMu = table.Column<bool>(type: "bit", nullable: false),
                    bildirimZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bildirimGorulmeZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KullaniciId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bildirim", x => x.BildirimID);
                    table.ForeignKey(
                        name: "FK_Bildirim_AspNetUsers_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DestekTalepleri",
                columns: table => new
                {
                    DestekID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestekTalebiAcanId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Konu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcilmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGuncelleme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KapanmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    kapandimi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestekTalepleri", x => x.DestekID);
                    table.ForeignKey(
                        name: "FK_DestekTalepleri_AspNetUsers_DestekTalebiAcanId",
                        column: x => x.DestekTalebiAcanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Izinler",
                columns: table => new
                {
                    IzinID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IzinAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RolId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izinler", x => x.IzinID);
                    table.ForeignKey(
                        name: "FK_Izinler_AspNetRoles_RolId",
                        column: x => x.RolId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    KategoriID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kisaad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KategoriSimgesi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KategoriAciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KategoriID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.KategoriID);
                    table.ForeignKey(
                        name: "FK_Kategoriler_Kategoriler_KategoriID1",
                        column: x => x.KategoriID1,
                        principalTable: "Kategoriler",
                        principalColumn: "KategoriID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Marketler",
                columns: table => new
                {
                    MarketID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contactSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TCKIMLIKNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isbaslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubMerchantKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OneCikanMarket = table.Column<bool>(type: "bit", nullable: false),
                    Begeni = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marketler", x => x.MarketID);
                });

            migrationBuilder.CreateTable(
                name: "MesajAraTablo",
                columns: table => new
                {
                    KimeMesajYollandiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KullaniciId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MesajAraTablo", x => x.KimeMesajYollandiID);
                    table.ForeignKey(
                        name: "FK_MesajAraTablo_AspNetUsers_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mesajlar",
                columns: table => new
                {
                    MesajID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MesajGovde = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OkunduMu = table.Column<bool>(type: "bit", nullable: false),
                    UstMesajID = table.Column<int>(type: "int", nullable: false),
                    Yollayan_KullaniciId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Alan_KullaniciId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    YollanmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OkunmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TelefonNumarasiIceriyor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesajlar", x => x.MesajID);
                    table.ForeignKey(
                        name: "FK_Mesajlar_AspNetUsers_Alan_KullaniciId",
                        column: x => x.Alan_KullaniciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mesajlar_AspNetUsers_Yollayan_KullaniciId",
                        column: x => x.Yollayan_KullaniciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MessageText = table.Column<string>(type: "nvarchar(144)", maxLength: 144, nullable: true),
                    MesssageDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                });

            migrationBuilder.CreateTable(
                name: "SiteAyari",
                columns: table => new
                {
                    SiteAyariID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IyzicoApiSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IyzicoApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    komisyon = table.Column<int>(type: "int", nullable: false),
                    IyzicoBaseURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAyari", x => x.SiteAyariID);
                });

            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    UrunID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunBaslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrunAktifmi = table.Column<bool>(type: "bit", nullable: false),
                    UrunYaratilmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrunDegismeZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrunAciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrunFiyat = table.Column<int>(type: "int", nullable: false),
                    SatilmaSayisi = table.Column<int>(type: "int", nullable: false),
                    UrunSahibiId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BegeniNumarasi = table.Column<int>(type: "int", nullable: false),
                    Urunkisaad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kisaTanitim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OneCikarilan = table.Column<bool>(type: "bit", nullable: false),
                    KategoriAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KategoriID = table.Column<int>(type: "int", nullable: true),
                    MarketID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.UrunID);
                    table.ForeignKey(
                        name: "FK_Urunler_AspNetUsers_UrunSahibiId",
                        column: x => x.UrunSahibiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Urunler_Kategoriler_KategoriID",
                        column: x => x.KategoriID,
                        principalTable: "Kategoriler",
                        principalColumn: "KategoriID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Urunler_Marketler_MarketID",
                        column: x => x.MarketID,
                        principalTable: "Marketler",
                        principalColumn: "MarketID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resimler",
                columns: table => new
                {
                    ResimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResimAciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KucukResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrunID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resimler", x => x.ResimID);
                    table.ForeignKey(
                        name: "FK_Resimler_Urunler_UrunID",
                        column: x => x.UrunID,
                        principalTable: "Urunler",
                        principalColumn: "UrunID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Satislar",
                columns: table => new
                {
                    SatisID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SatilanUrunUrunID = table.Column<int>(type: "int", nullable: true),
                    Satan_MarketMarketID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Alan_KullaniciId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    odenenPara = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    iyzicoToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    istek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    odendiMi = table.Column<bool>(type: "bit", nullable: false),
                    AlinmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OnaylanmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    iptalTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Onaylandimi = table.Column<bool>(type: "bit", nullable: false),
                    iadeEdildimi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Satislar", x => x.SatisID);
                    table.ForeignKey(
                        name: "FK_Satislar_AspNetUsers_Alan_KullaniciId",
                        column: x => x.Alan_KullaniciId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Satislar_Marketler_Satan_MarketMarketID",
                        column: x => x.Satan_MarketMarketID,
                        principalTable: "Marketler",
                        principalColumn: "MarketID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Satislar_Urunler_SatilanUrunUrunID",
                        column: x => x.SatilanUrunUrunID,
                        principalTable: "Urunler",
                        principalColumn: "UrunID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Yorumlar",
                columns: table => new
                {
                    YorumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YorumuYazanId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    YorumunIcerigi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerilenPuan = table.Column<int>(type: "int", nullable: false),
                    AltCevapYorumID = table.Column<int>(type: "int", nullable: true),
                    YorumYazilma = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YorumDegistirilme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YorumSilindimi = table.Column<bool>(type: "bit", nullable: false),
                    MarketID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UrunID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yorumlar", x => x.YorumID);
                    table.ForeignKey(
                        name: "FK_Yorumlar_AspNetUsers_YorumuYazanId",
                        column: x => x.YorumuYazanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Marketler_MarketID",
                        column: x => x.MarketID,
                        principalTable: "Marketler",
                        principalColumn: "MarketID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Urunler_UrunID",
                        column: x => x.UrunID,
                        principalTable: "Urunler",
                        principalColumn: "UrunID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Yorumlar_AltCevapYorumID",
                        column: x => x.AltCevapYorumID,
                        principalTable: "Yorumlar",
                        principalColumn: "YorumID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogYazilari",
                columns: table => new
                {
                    BlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogYazisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YazilmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlogResimResimID = table.Column<int>(type: "int", nullable: true),
                    Etiketler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OkumaDakikasi = table.Column<int>(type: "int", nullable: false),
                    linkBaslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Begenenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Begenmeyenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KalpAtanlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MutluOlanlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aglayanlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gulenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WowDiyenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sinirlenenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YaziKisaAciklama = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogYazilari", x => x.BlogID);
                    table.ForeignKey(
                        name: "FK_BlogYazilari_Resimler_BlogResimResimID",
                        column: x => x.BlogResimResimID,
                        principalTable: "Resimler",
                        principalColumn: "ResimID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Durumlar",
                columns: table => new
                {
                    DurumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    durumIcerik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DestekID = table.Column<int>(type: "int", nullable: true),
                    SatisID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Durumlar", x => x.DurumID);
                    table.ForeignKey(
                        name: "FK_Durumlar_DestekTalepleri_DestekID",
                        column: x => x.DestekID,
                        principalTable: "DestekTalepleri",
                        principalColumn: "DestekID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Durumlar_Satislar_SatisID",
                        column: x => x.SatisID,
                        principalTable: "Satislar",
                        principalColumn: "SatisID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogYorumlar",
                columns: table => new
                {
                    BlogYorumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YorumYapanKullanici = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YorumunIcerigi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AltYorumBlogYorumID = table.Column<int>(type: "int", nullable: true),
                    YorumYazilma = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YorumDegistirilme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YorumSilindimi = table.Column<bool>(type: "bit", nullable: false),
                    Begenenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Begenmeyenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KalpAtanlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MutluOlanlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aglayanlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gulenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WowDiyenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sinirlenenler = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogYorumlar", x => x.BlogYorumID);
                    table.ForeignKey(
                        name: "FK_BlogYorumlar_BlogYazilari_BlogID",
                        column: x => x.BlogID,
                        principalTable: "BlogYazilari",
                        principalColumn: "BlogID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogYorumlar_BlogYorumlar_AltYorumBlogYorumID",
                        column: x => x.AltYorumBlogYorumID,
                        principalTable: "BlogYorumlar",
                        principalColumn: "BlogYorumID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarResimID",
                table: "AspNetUsers",
                column: "AvatarResimID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MarketProfiliMarketID",
                table: "AspNetUsers",
                column: "MarketProfiliMarketID");

            migrationBuilder.CreateIndex(
                name: "IX_Bildirim_KullaniciId",
                table: "Bildirim",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogYazilari_BlogResimResimID",
                table: "BlogYazilari",
                column: "BlogResimResimID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogYorumlar_AltYorumBlogYorumID",
                table: "BlogYorumlar",
                column: "AltYorumBlogYorumID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogYorumlar_BlogID",
                table: "BlogYorumlar",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_DestekTalepleri_DestekTalebiAcanId",
                table: "DestekTalepleri",
                column: "DestekTalebiAcanId");

            migrationBuilder.CreateIndex(
                name: "IX_Durumlar_DestekID",
                table: "Durumlar",
                column: "DestekID");

            migrationBuilder.CreateIndex(
                name: "IX_Durumlar_SatisID",
                table: "Durumlar",
                column: "SatisID");

            migrationBuilder.CreateIndex(
                name: "IX_Izinler_RolId",
                table: "Izinler",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Kategoriler_KategoriID1",
                table: "Kategoriler",
                column: "KategoriID1");

            migrationBuilder.CreateIndex(
                name: "IX_MesajAraTablo_KullaniciId",
                table: "MesajAraTablo",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_Alan_KullaniciId",
                table: "Mesajlar",
                column: "Alan_KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_Yollayan_KullaniciId",
                table: "Mesajlar",
                column: "Yollayan_KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Resimler_UrunID",
                table: "Resimler",
                column: "UrunID");

            migrationBuilder.CreateIndex(
                name: "IX_Satislar_Alan_KullaniciId",
                table: "Satislar",
                column: "Alan_KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Satislar_Satan_MarketMarketID",
                table: "Satislar",
                column: "Satan_MarketMarketID");

            migrationBuilder.CreateIndex(
                name: "IX_Satislar_SatilanUrunUrunID",
                table: "Satislar",
                column: "SatilanUrunUrunID");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_KategoriID",
                table: "Urunler",
                column: "KategoriID");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_MarketID",
                table: "Urunler",
                column: "MarketID");

            migrationBuilder.CreateIndex(
                name: "IX_Urunler_UrunSahibiId",
                table: "Urunler",
                column: "UrunSahibiId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_AltCevapYorumID",
                table: "Yorumlar",
                column: "AltCevapYorumID");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_MarketID",
                table: "Yorumlar",
                column: "MarketID");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_UrunID",
                table: "Yorumlar",
                column: "UrunID");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_YorumuYazanId",
                table: "Yorumlar",
                column: "YorumuYazanId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Marketler_MarketProfiliMarketID",
                table: "AspNetUsers",
                column: "MarketProfiliMarketID",
                principalTable: "Marketler",
                principalColumn: "MarketID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Resimler_AvatarResimID",
                table: "AspNetUsers",
                column: "AvatarResimID",
                principalTable: "Resimler",
                principalColumn: "ResimID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Marketler_MarketProfiliMarketID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Resimler_AvatarResimID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Bildirim");

            migrationBuilder.DropTable(
                name: "BlogYorumlar");

            migrationBuilder.DropTable(
                name: "Durumlar");

            migrationBuilder.DropTable(
                name: "Izinler");

            migrationBuilder.DropTable(
                name: "MesajAraTablo");

            migrationBuilder.DropTable(
                name: "Mesajlar");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "SiteAyari");

            migrationBuilder.DropTable(
                name: "Yorumlar");

            migrationBuilder.DropTable(
                name: "BlogYazilari");

            migrationBuilder.DropTable(
                name: "DestekTalepleri");

            migrationBuilder.DropTable(
                name: "Satislar");

            migrationBuilder.DropTable(
                name: "Resimler");

            migrationBuilder.DropTable(
                name: "Urunler");

            migrationBuilder.DropTable(
                name: "Kategoriler");

            migrationBuilder.DropTable(
                name: "Marketler");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AvatarResimID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MarketProfiliMarketID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AktifMi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AktifOlmamaNedeni",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AvatarResimID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DegistirilmeTarihi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EnSonOturumAcma",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MarketHesabimi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MarketProfiliMarketID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OlusturulmaTarihi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Soyad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Telefon_Numarasi",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Rol_AD",
                table: "AspNetRoles");
        }
    }
}

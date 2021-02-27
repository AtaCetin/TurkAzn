using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TurkAzn.Data;
using TurkAzn.Data.Kimlik;

namespace TurkAzn.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TurkAzn.Data.Kimlik.Kullanici> Kullanicilar { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<SiteAyari> SiteAyari { get; set; }
        public DbSet<Market> Marketler { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Resim> Resimler { get; set; }
        public DbSet<Izin> Izinler { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Mesaj> Mesajlar { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<KimeMesajYollandi> MesajAraTablo { get; set; }
        public DbSet<Satis> Satislar { get; set; }
        public DbSet<Adres> Adresler { get; set; }
        public DbSet<Durum> Durumlar { get; set; }
        public DbSet<Destek> DestekTalepleri { get; set; }
        public DbSet<BlogYorum> BlogYorumlar { get; set; }
        public DbSet<Blog> BlogYazilari { get; set; }
        public DbSet<ShoppingCartItem> SepetEsyasi { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TurkAzn.Data;
using TurkAzn.Data.Kimlik;
using TurkAzn.Desktop.Data;

namespace TurkAzn.Desktop.Forms
{
    public partial class frmUrunEkle : Form
    {
        private ApplicationDbContext appDb;
        public frmUrunEkle(ApplicationDbContext _context)
        {
            appDb = _context;
            InitializeComponent();
            cbKategori.DataSource = appDb.Kategoriler.ToList();
            cbUrun.DataSource = appDb.Marketler.ToList();
            cbUrun.DisplayMember = "kisaAd";
            cbKategori.DisplayMember = "KategoriAd";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Urun u = new Urun();
            u.KategoriAdi = cbKategori.Text;
            u.UrunAciklama = rtbAciklama.Rtf;
            u.UrunBaslik = tbUrunBaslik.Text;
            u.UrunDegismeZamani = DateTime.Now;
            u.UrunFiyat = Convert.ToDecimal(tbFiyat.Text);
            u.Urunkisaad = tbKisaAd.Text;
            u.UrunSahibi = cbUrun.SelectedItem as Market;
            u.UrunYaratilmaZamani = DateTime.Now;
            
            appDb.Urunler.Add(u);
            await appDb.SaveChangesAsync();

            var kategori = cbKategori.SelectedItem as Kategori;
            
            kategori.kategoridekiUrunler.Add(u);
            appDb.Kategoriler.Update(kategori);
            
            await appDb.SaveChangesAsync();

            MessageBox.Show("Ekleme Başarılı");
            this.Close();
        }
    }
}

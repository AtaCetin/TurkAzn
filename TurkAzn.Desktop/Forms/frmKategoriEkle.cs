using System;
using System.Linq;
using System.Windows.Forms;
using TurkAzn.Data;
using TurkAzn.Desktop.Data;

namespace TurkAzn.Desktop.Forms
{
    public partial class frmKategoriEkle : Form
    {
        private ApplicationDbContext appDb;
        public frmKategoriEkle(ApplicationDbContext _context)
        {
            appDb = _context;
            InitializeComponent();
            cbKategori.DataSource = appDb.Kategoriler.Where(x => x.AnaKategoriMi == true).ToList();
            cbKategori.DisplayMember = "KategoriAd";
        }


        private void btnEkle_Click(object sender, EventArgs e)
        {
            Kategori k = new Kategori
            {
                kisaad = tbKisaAd.Text,
                KategoriAciklama = tbKategoriAciklama.Text,
                KategoriAd = tbKategoriAD.Text,
                KategoriSimgesi = tbSimgesi.Text,
                AnaKategoriMi = cbAnaKategori.Checked
            };

            appDb.Kategoriler.Add(k);
            appDb.SaveChanges();

            if (!cbAnaKategori.Checked)
            {
                Kategori secilen = cbKategori.SelectedItem as Kategori; 
                secilen.KategorininAlt.Add(k); 
                appDb.Kategoriler.Update(secilen); 
                appDb.SaveChanges();
            }
            
            this.Close();
        }

        private void frmKategoriEkle_Load(object sender, EventArgs e)
        {
            
        }

        private void cbAnaKategori_CheckedChanged(object sender, EventArgs e)
        {
            var ck = sender as CheckBox;
            cbKategori.Enabled = !ck.Checked;
        }
    }
}
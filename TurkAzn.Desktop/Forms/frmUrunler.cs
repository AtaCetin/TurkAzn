using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TurkAzn.Desktop.Data;

namespace TurkAzn.Desktop.Forms
{
    public partial class frmUrunler : Form
    {
        private ApplicationDbContext appDb;
        public frmUrunler(ApplicationDbContext _context)
        {
            appDb = _context;
            InitializeComponent();
            
            dGVurunler.DataSource = appDb.Urunler.ToList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            frmUrunEkle frm = new frmUrunEkle(appDb);
            frm.Show();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            dGVurunler.DataSource = appDb.Urunler.ToList();
        }
    }
}

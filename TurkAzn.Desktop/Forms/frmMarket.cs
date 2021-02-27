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
    public partial class frmMarket : Form
    {
        private ApplicationDbContext appDb;
        public frmMarket(ApplicationDbContext _context)
        {
            appDb = _context;
            InitializeComponent();
            dGVmarket.DataSource = appDb.Marketler.ToList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            frmMarketEkle frm = new frmMarketEkle(appDb);
            frm.Show();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            dGVmarket.DataSource = appDb.Marketler.ToList();
        }
    }
}

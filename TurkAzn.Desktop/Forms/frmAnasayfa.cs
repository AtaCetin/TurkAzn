using Microsoft.EntityFrameworkCore;
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
    public partial class frmAnasayfa : Form
    {
        private ApplicationDbContext appDb;

        public frmAnasayfa()
        {
            
            appDb = new ApplicationDbContext("Data Source=104.238.176.209;database=TurkAzn;User ID=sa;Password=123OnLive123;Connect Timeout=15;Packet Size=4096;MultipleActiveResultSets=true");
            InitializeComponent();
        }

        private void btnMarket_Click(object sender, EventArgs e)
        {
            frmMarket frmMarket = new frmMarket(appDb);
            frmMarket.Show();
        }

        private void btnUrunler_Click(object sender, EventArgs e)
        {
            frmUrunler frmUrunler = new frmUrunler(appDb);
            frmUrunler.Show();
        }

        private void btnKategori_Click(object sender, EventArgs e)
        {
            frmKategori frmKategori = new frmKategori(appDb);
            frmKategori.Show();
        }
    }
}

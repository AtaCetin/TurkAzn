using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using TurkAzn.Desktop.Data;

namespace TurkAzn.Desktop.Forms
{
    public partial class frmKategori : Form
    {
        
        private ApplicationDbContext appDb;
        public frmKategori(ApplicationDbContext _context)
        {
            appDb = _context;
            InitializeComponent();
            //dgvKategori.DataSource = appDb.Kategoriler.ToList();
            
            TreeviewDoldur();
        }

        private void TreeviewDoldur()
        {
            TreeNode treeNode = new TreeNode("Kategoriler");
            treeView1.Nodes.Add(treeNode);
            foreach (var item in appDb.Kategoriler.Include(x => x.KategorininAlt).Where(x => x.AnaKategoriMi))
            {
                TreeNode nodes = new TreeNode("Kategori : " + item.KategoriID);
                TreeNode childNode1 = new TreeNode("Ad: " + item.KategoriAd);
                TreeNode childNode2 = new TreeNode("Kisa Ad: " + item.kisaad);
                TreeNode childNode3 = new TreeNode("Ana Kategori mi: " + item.AnaKategoriMi);
                TreeNode childNode4 = new TreeNode("Açıklama : " + item.KategoriAciklama);
                nodes.Nodes.Add(childNode1);
                nodes.Nodes.Add(childNode2);
                nodes.Nodes.Add(childNode3);
                nodes.Nodes.Add(childNode4);
                foreach (var item2 in item.KategorininAlt)
                {
                    TreeNode childNode = new TreeNode("Alt Kategori: " + item2.KategoriID);
                    TreeNode childNode5 = new TreeNode("Ad: " + item2.KategoriAd);
                    TreeNode childNode6 = new TreeNode("Kisa Ad: " + item2.kisaad);
                    TreeNode childNode7 = new TreeNode("Ana Kategori mi: " + item2.AnaKategoriMi);
                    TreeNode childNode8 = new TreeNode("Açıklama : " + item2.KategoriAciklama);
                    childNode.Nodes.Add(childNode5);
                    childNode.Nodes.Add(childNode6);
                    childNode.Nodes.Add(childNode7);
                    childNode.Nodes.Add(childNode8);
                    nodes.Nodes.Add(childNode);
                }

                treeNode.Nodes.Add(nodes);
            }
        }

        private void dGVmarket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void frmKategori_Load(object sender, EventArgs e)
        {
            
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            frmKategoriEkle fKE = new frmKategoriEkle(appDb);
            fKE.Show();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            TreeviewDoldur();
        }
    }
}
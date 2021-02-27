
namespace TurkAzn.Desktop.Forms
{
    partial class frmAnasayfa
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnMarket = new System.Windows.Forms.Button();
            this.btnUrunler = new System.Windows.Forms.Button();
            this.btnKategori = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMarket
            // 
            this.btnMarket.Location = new System.Drawing.Point(611, 360);
            this.btnMarket.Name = "btnMarket";
            this.btnMarket.Size = new System.Drawing.Size(64, 20);
            this.btnMarket.TabIndex = 0;
            this.btnMarket.Text = "Marketler";
            this.btnMarket.UseVisualStyleBackColor = true;
            this.btnMarket.Click += new System.EventHandler(this.btnMarket_Click);
            // 
            // btnUrunler
            // 
            this.btnUrunler.Location = new System.Drawing.Point(542, 360);
            this.btnUrunler.Name = "btnUrunler";
            this.btnUrunler.Size = new System.Drawing.Size(64, 20);
            this.btnUrunler.TabIndex = 1;
            this.btnUrunler.Text = "Ürünler";
            this.btnUrunler.UseVisualStyleBackColor = true;
            this.btnUrunler.Click += new System.EventHandler(this.btnUrunler_Click);
            // 
            // btnKategori
            // 
            this.btnKategori.Location = new System.Drawing.Point(461, 359);
            this.btnKategori.Name = "btnKategori";
            this.btnKategori.Size = new System.Drawing.Size(75, 23);
            this.btnKategori.TabIndex = 2;
            this.btnKategori.Text = "Kategori";
            this.btnKategori.UseVisualStyleBackColor = true;
            this.btnKategori.Click += new System.EventHandler(this.btnKategori_Click);
            // 
            // frmAnasayfa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 390);
            this.Controls.Add(this.btnKategori);
            this.Controls.Add(this.btnUrunler);
            this.Controls.Add(this.btnMarket);
            this.Name = "frmAnasayfa";
            this.Text = "TurkAzn - Anasayfa";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnKategori;

        #endregion

        private System.Windows.Forms.Button btnMarket;
        private System.Windows.Forms.Button btnUrunler;
    }
}


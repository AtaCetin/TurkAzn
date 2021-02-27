using System.ComponentModel;

namespace TurkAzn.Desktop.Forms
{
    partial class frmKategoriEkle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            this.tbKategoriAD = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbKisaAd = new System.Windows.Forms.TextBox();
            this.cbAnaKategori = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbKategoriAciklama = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSimgesi = new System.Windows.Forms.TextBox();
            this.btnEkle = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbKategori = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // tbKategoriAD
            // 
            this.tbKategoriAD.Location = new System.Drawing.Point(88, 20);
            this.tbKategoriAD.Name = "tbKategoriAD";
            this.tbKategoriAD.Size = new System.Drawing.Size(100, 20);
            this.tbKategoriAD.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kategori Adı";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Kısa Adı:";
            // 
            // tbKisaAd
            // 
            this.tbKisaAd.Location = new System.Drawing.Point(88, 46);
            this.tbKisaAd.Name = "tbKisaAd";
            this.tbKisaAd.Size = new System.Drawing.Size(100, 20);
            this.tbKisaAd.TabIndex = 2;
            // 
            // cbAnaKategori
            // 
            this.cbAnaKategori.Location = new System.Drawing.Point(12, 75);
            this.cbAnaKategori.Name = "cbAnaKategori";
            this.cbAnaKategori.Size = new System.Drawing.Size(176, 24);
            this.cbAnaKategori.TabIndex = 4;
            this.cbAnaKategori.Text = "Ana kategori mi?";
            this.cbAnaKategori.UseVisualStyleBackColor = true;
            this.cbAnaKategori.CheckedChanged += new System.EventHandler(this.cbAnaKategori_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Açıklama:";
            // 
            // tbKategoriAciklama
            // 
            this.tbKategoriAciklama.Location = new System.Drawing.Point(88, 105);
            this.tbKategoriAciklama.Name = "tbKategoriAciklama";
            this.tbKategoriAciklama.Size = new System.Drawing.Size(318, 20);
            this.tbKategoriAciklama.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 23);
            this.label4.TabIndex = 8;
            this.label4.Text = "Resmi:";
            // 
            // tbSimgesi
            // 
            this.tbSimgesi.Location = new System.Drawing.Point(88, 131);
            this.tbSimgesi.Name = "tbSimgesi";
            this.tbSimgesi.Size = new System.Drawing.Size(318, 20);
            this.tbSimgesi.TabIndex = 7;
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(331, 157);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(75, 23);
            this.btnEkle.TabIndex = 9;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(217, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Üst Kategori";
            // 
            // cbKategori
            // 
            this.cbKategori.FormattingEnabled = true;
            this.cbKategori.Location = new System.Drawing.Point(217, 45);
            this.cbKategori.Name = "cbKategori";
            this.cbKategori.Size = new System.Drawing.Size(187, 21);
            this.cbKategori.TabIndex = 25;
            // 
            // frmKategoriEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 187);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbKategori);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbSimgesi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbKategoriAciklama);
            this.Controls.Add(this.cbAnaKategori);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbKisaAd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbKategoriAD);
            this.Name = "frmKategoriEkle";
            this.Text = "frmKategoriEkle";
            this.Load += new System.EventHandler(this.frmKategoriEkle_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox cbKategori;
        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSimgesi;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbKategoriAciklama;

        private System.Windows.Forms.CheckBox cbAnaKategori;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbKategoriAD;
        private System.Windows.Forms.TextBox tbKisaAd;

        #endregion
    }
}
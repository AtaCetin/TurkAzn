using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
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
    public partial class frmMarketEkle : Form
    {
        private ApplicationDbContext appDb;
        List<string> MarketTipi;
        List<string> ParaTipi;
        public frmMarketEkle(ApplicationDbContext _context)
        {
            appDb = _context;
            MarketTipi = new List<string>();
            ParaTipi = new List<string>();
            InitializeComponent();
            cbKullanici.DataSource = appDb.Kullanicilar.ToList();
            MarketTipi.Add(SubMerchantType.LIMITED_OR_JOINT_STOCK_COMPANY.ToString());
            MarketTipi.Add(SubMerchantType.PERSONAL.ToString());
            MarketTipi.Add(SubMerchantType.PRIVATE_COMPANY.ToString());
            cbMarketTip.DataSource = MarketTipi;
            ParaTipi.Add(Currency.CHF.ToString());
            ParaTipi.Add(Currency.EUR.ToString());
            ParaTipi.Add(Currency.GBP.ToString());
            ParaTipi.Add(Currency.IRR.ToString());
            ParaTipi.Add(Currency.NOK.ToString());
            ParaTipi.Add(Currency.RUB.ToString());
            ParaTipi.Add(Currency.TRY.ToString());
            ParaTipi.Add(Currency.USD.ToString());
            cbKullanici.DisplayMember = "Email";
            cbPara.DataSource = ParaTipi;
        }

        private async void btnEkle_Click(object sender, EventArgs e)
        {
            var ayar = appDb.SiteAyari.Find(1);
            var kullanici = cbKullanici.SelectedItem as Kullanici;

            CreateSubMerchantRequest request = new CreateSubMerchantRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = kullanici.Id + "_Istek",
                SubMerchantExternalId = kullanici.Id,
                SubMerchantType = cbMarketTip.SelectedItem.ToString(),
                Address = tbAdress.Text,
                ContactName = tbAd.Text,
                ContactSurname = tbSoyad.Text,
                Email = kullanici.Email,
                GsmNumber = kullanici.Telefon_Numarasi,
                Name = kullanici.UserName,
                Iban = tbIban.Text,
                IdentityNumber = tbTcKimlik.Text,
                LegalCompanyTitle = tbLegal.Text,
                TaxNumber = tbVergiNumarasi.Text,
                TaxOffice = tbVergiDairesi.Text,
                Currency = cbPara.SelectedItem.ToString()
            };

            SubMerchant subMerchant = SubMerchant.Create(request, new Options() { ApiKey = ayar.IyzicoApiKey, BaseUrl = ayar.IyzicoBaseURL, SecretKey = ayar.IyzicoApiSecret });

            Market m = new Market()
            {
                 IBAN = tbIban.Text, Aciklama = tbAciklama.Text, Adress = tbAdress.Text , contactName = tbAd.Text, contactSurname = tbSoyad.Text, legalCompanyTitle = tbLegal.Text,  MarketTipi = cbMarketTip.SelectedItem.ToString(), ParaTipi = cbPara.SelectedItem.ToString(), TCKIMLIKNO = tbTcKimlik.Text, VergiDairesi = tbVergiDairesi.Text, VergiNumarasi = tbVergiNumarasi.Text 
            };


            if (subMerchant.Status == Status.SUCCESS.ToString())
            {
                m.MarketID = kullanici.Id;
                m.SubMerchantKey = subMerchant.SubMerchantKey;
                await appDb.Marketler.AddAsync(m);
                kullanici.MarketHesabimi = true;
                kullanici.MarketProfili = m;
                appDb.Kullanicilar.Update(kullanici);
                await appDb.SaveChangesAsync();

                MessageBox.Show("Kayıt İşlemi Tamamlandı!", "Bilgilendirme Penceresi");
                this.Close();
            }
            else
            {
                string mesaj = "Iyzico bir hata döndürdü \"" + subMerchant.ErrorMessage + "\" Hata kodu: " +
                               subMerchant.ErrorCode;
                MessageBox.Show(mesaj, "Hata");
            }
        }
    }
}

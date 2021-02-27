using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TurkAzn.Data.Kimlik;

namespace TurkAzn.Data
{

    public class KimeMesajYollandi
    {
        public int KimeMesajYollandiID { get; set; }
        public string KullaniciAdi { get; set; }
    }


    public class Message
    {
        [Key] public int MessageId { get; set; }
        [MaxLength(50)] public string Username { get; set; }
        [MaxLength(144)] public string MessageText { get; set; }
        public DateTime MesssageDate { get; set; }
    }


    public class Blog
    {
        public Blog()
        {
            if (BlogYorumlari == null)
            {
                BlogYorumlari = new List<BlogYorum>();
            }
        }

        public int BlogID { get; set; }

        public string Baslik { get; set; }

        public string BlogYazisi { get; set; }

        public DateTime YazilmaZamani { get; set; }

        public Resim BlogResim { get; set; }

        public string Etiketler { get; set; }

        public int OkumaDakikasi { get; set; }

        public string linkBaslik { get; set; }

        public string Begenenler { get; set; }

        public string Begenmeyenler { get; set; }

        public string KalpAtanlar { get; set; }
        public string MutluOlanlar { get; set; }
        public string Aglayanlar { get; set; }
        public string Gulenler { get; set; }
        public string WowDiyenler { get; set; }
        public string Sinirlenenler { get; set; }

        public string YaziKisaAciklama { get; set; }

        public List<BlogYorum> BlogYorumlari { get; set; }
    }

    public class BlogYorum
    {
        public int BlogYorumID { get; set; }
        public string YorumYapanKullanici { get; set; }
        public string YorumunIcerigi { get; set; }
        public BlogYorum AltYorum { get; set; }
        public DateTime YorumYazilma { get; set; }
        public DateTime YorumDegistirilme { get; set; }
        public bool YorumSilindimi { get; set; }

        public string Begenenler { get; set; }

        public string Begenmeyenler { get; set; }

        public string KalpAtanlar { get; set; }
        public string MutluOlanlar { get; set; }
        public string Aglayanlar { get; set; }
        public string Gulenler { get; set; }
        public string WowDiyenler { get; set; }
        public string Sinirlenenler { get; set; }
    }

    public class Bildirim
    {
        public int BildirimID { get; set; }
        public string BildirimIcerik { get; set; }

        public string BildirimYollayan { get; set; }

        public string bildirimLink { get; set; }

        public bool okunduMu { get; set; }

        public DateTime bildirimZamani { get; set; }
        public DateTime bildirimGorulmeZamani { get; set; }

    }

    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public Urun Urun { get; set; }
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; }
    }
    
    public class Adres
    {
        public int adresID { get; set; }
        public string city { get; set; }
        public string AdresName { get; set; }
        public string ilce { get; set; }
        public string description { get; set; }
        public string namesurname { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
    }
    

    public class Kategori
    {
        public Kategori()
        {
            if (KategorininAlt == null)
            {
                KategorininAlt = new List<Kategori>();
            }
            if(kategoridekiUrunler == null)
            {
                kategoridekiUrunler = new List<Urun>();
            }
        }

        public int KategoriID { get; set; }
        public string KategoriAd { get; set; }
        public string kisaad { get; set; }
        public bool AnaKategoriMi {get; set;}
        public string KategoriSimgesi { get; set; }

        public string KategoriAciklama { get; set; }
        public List<Urun> kategoridekiUrunler { get; set; }
        public List<Kategori> KategorininAlt { get; set; }
        public List<Reklam> KategoriReklami { get; set; }
        public List<Market> OneCikanMarka { get; set; }
    }


    public class Reklam
    {
        public int ReklamID { get; set; }
        public string ReklamAciklama { get; set; }
        public string ReklamResimYolu { get; set; }
        public string KucukReklamResimYolu { get; set; }
    }


    public class Destek
    {
        public int DestekID { get; set; }
        public Kullanici DestekTalebiAcan { get; set; }
        public string Konu { get; set; }
        public string Mesaj { get; set; }
        public DateTime AcilmaZamani { get; set; }
        public DateTime SonGuncelleme { get; set; }
        public DateTime KapanmaZamani { get; set; }
        public bool kapandimi { get; set; }
        public List<Durum> DestekDurum { get; set; }
    }

    public class Satis
    {
        public Satis()
        {
            SatisDurumu = new List<Durum>();
        }

        public int SatisID { get; set; }

        public string SepetID { get; set; }

        public string KargoNum { get; set; }
        public bool KargoYollandimi { get; set; }
        public KargoSirketi YollananKargoSirketi { get; set; }
        
        public int miktar { get; set; }
        
        public Urun SatilanUrun { get; set; }
        public Market Satan_Market { get; set; }
        public Kullanici Alan_Kullanici { get; set; }
        public decimal odenenPara { get; set; }
        public string iyzicoToken { get; set; }
        
        public Adres verilenAdres { get; set; }
        
        public bool odendiMi { get; set; }
        public DateTime AlinmaTarihi { get; set; }
        public DateTime OnaylanmaTarihi { get; set; }
        public DateTime iptalTarihi { get; set; }
        public bool Onaylandimi { get; set; }
        public List<Durum> SatisDurumu { get; set; }
        public bool iadeEdildimi { get; set; }
    }

    public class KargoSirketi
    {
        public int KargoSirketiID { get; set; }
        public string Adi { get; set; }
        public string Logo { get; set; }
        public string Kontrollinki { get; set; }
    }
    
    
    
    public class Durum
    {
        public int DurumID { get; set; }
        public string durumIcerik { get; set; }
        public DateTime DurumTarihi { get; set; }
    }

    public class Mesaj
    {
        public int MesajID { get; set; }
        public string MesajGovde { get; set; }
        public bool OkunduMu { get; set; }
        public int UstMesajID { get; set; }
        public Kullanici Yollayan_Kullanici { get; set; }
        public Kullanici Alan_Kullanici { get; set; }
        public DateTime YollanmaZamani { get; set; }
        public DateTime OkunmaZamani { get; set; }
        public bool TelefonNumarasiIceriyor { get; set; }
    }

    public class Market
    {
        public Market()
        {   
            if (Urunler == null)
            {
                Urunler = new List<Urun>();
            }
        }
        public string MarketID { get; set; }
        public string Aciklama { get; set; }
        public string contactName { get; set; }
        public string contactSurname { get; set; }
        public string IBAN { get; set; }
        public string Adress { get; set; }
        public string TCKIMLIKNO { get; set; }
        public string legalCompanyTitle { get; set; }
        public string SubMerchantKey { get; set; }
        public string MarketTipi {get; set;}
        public string VergiDairesi {get; set;}
        public string VergiNumarasi {get; set;}
        public string ParaTipi {get; set;}
        public string kisaAd { get; set; }

        public bool OneCikanMarket { get; set; }
        public Resim Logo { get; set; }      

        public int Begeni { get; set; }
        public List<Yorum> AldigiYorumlar { get; set; }
        public List<Urun> Urunler { get; set; }
    }

    public class Urun
    {
        public int UrunID { get; set; }
        public string UrunBaslik { get; set; }
        public bool UrunAktifmi { get; set; }
        public DateTime UrunYaratilmaZamani { get; set; }
        public DateTime UrunDegismeZamani { get; set; }
        public string UrunAciklama { get; set; }
        public decimal UrunFiyat { get; set; }
        public int SatilmaSayisi { get; set; }
        public Market UrunSahibi { get; set; }
        public List<Yorum> UrunYorumlari { get; set; }
        public int BegeniNumarasi { get; set; }
        public string Urunkisaad { get; set; }
        public string kisaTanitim { get; set; }
        public bool OneCikarilan { get; set; }
        public List<Resim> UrunResimleri { get; set; }
        public string KategoriAdi { get; set; }
        public int StokSayisi  { get; set; }

    }


    public class Yorum
    {
        public int YorumID { get; set; }
        public Kullanici YorumuYazan { get; set; }
        public string YorumunIcerigi { get; set; }
        public int VerilenPuan { get; set; }
        public Yorum AltCevap { get; set; }
        public DateTime YorumYazilma { get; set; }
        public DateTime YorumDegistirilme { get; set; }
        public bool YorumSilindimi { get; set; }
    }

    public class Resim
    {
        public int ResimID { get; set; }
        public string ResimAciklama { get; set; }
        public string ResimYolu { get; set; }
        public string KucukResimYolu { get; set; }
    }

    //public class Ekstra
    //{
    //    public int EkstraID { get; set; }
    //    public string Baslik { get; set; }
    //    public decimal Ucret { get; set; }
    //}

    public class SiteAyari
    {
        public int SiteAyariID { get; set; }
        public string IyzicoApiSecret { get; set; }
        public string IyzicoApiKey { get; set; }
        public int komisyon { get; set; }
        public string IyzicoBaseURL { get; set; }
    }


    //public class Paket
    //{
    //    public int PaketID { get; set; }
    //    public string PaketBaslik { get; set; }
    //    public string PaketIcerik { get; set; }
    //    public decimal Ucret { get; set; }
    //}

    public class Izin
    {
        public int IzinID { get; set; }
        public string IzinAd { get; set; }
    }


}
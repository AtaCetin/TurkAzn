using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TurkAzn.Data;
using System.ComponentModel.DataAnnotations.Schema;



namespace TurkAzn.Data.Kimlik
{
    public class Kullanici : IdentityUser
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Kimlik { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }
        public DateTime DegistirilmeTarihi { get; set; }
        public bool AktifMi { get; set; }
        public string AktifOlmamaNedeni { get; set; }
        public string Telefon_Numarasi { get; set; }
        public Resim Avatar { get; set; }
        public bool MarketHesabimi { get; set; }
        public Market MarketProfili { get; set; }
        public DateTime EnSonOturumAcma { get; set; }
        public List<KimeMesajYollandi> Mesajlar {get; set;}

        public List<Bildirim> kullanicininBildirimleri { get; set; }
        
        public List<Adres> Adresler { get; set; }
    }



}

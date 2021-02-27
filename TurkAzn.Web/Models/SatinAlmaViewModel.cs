using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TurkAzn.Data;

namespace TurkAzn.Web.Models
{
    public class SatinAlmaViewModel
    {
        public Adres SecilenAdres { get; set; }
        public SepetIndexModel GelenSepet { get; set; }
        public TurkAzn.Data.Kimlik.Kullanici GelenKullanici { get; set; }
    }
}

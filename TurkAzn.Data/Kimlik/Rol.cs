using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace TurkAzn.Data.Kimlik
{
    public class Rol : IdentityRole
    {
        public string Rol_AD { get; set; }
        public List<Izin> Izinler { get; set; }
    }

}

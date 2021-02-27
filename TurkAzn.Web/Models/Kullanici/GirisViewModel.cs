using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurkAzn.Web.Models.Kullanici
{
    public class GirisViewModel
    {
        [Required]

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Display(Name = "Sifreyi hatırla")]
        public bool BeniHatirla { get; set; }
    }
}

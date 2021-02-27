using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurkAzn.Web.Models
{
    public class SifreSifirlamaVM
    {
        public string email { get; set; }
        public string Token { get; set; }
        [Required(ErrorMessage = "Hepsinin doldurulması zorunludur")]
        [StringLength(100, ErrorMessage = "Girilen şifre en az 6 karakter içermelidir.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string sifre { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrarı")]
        [Compare("sifre", ErrorMessage = "Girilen 2 şifre birbiriyle uyuşmuyor.")]
        public string sifreDogrulama { get; set; }
    }
}

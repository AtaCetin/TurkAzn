using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TurkAzn.Web.Models.Kullanici
{
    public class KullaniciBilgileriViewModel
    {
        [Required(ErrorMessage = "Hepsinin doldurulması zorunludur")]
        [Display(Name = "Ad")]
        public string Ad { get; set; }
        [Required(ErrorMessage = "Hepsinin doldurulması zorunludur")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }
        
        [Required(ErrorMessage = "Hepsinin doldurulması zorunludur")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(05(\d{9}))$", ErrorMessage = "Girmiş olduğunuz numara bir telefon numarasına benzemiyor")]
        [Display(Name = "Cep Telefonu")]
        public string TelefonNumarasi { get; set; }
        [Required(ErrorMessage = "Hepsinin doldurulması zorunludur")]
        [EmailAddress(ErrorMessage = "Girdiğiniz E-mail doğru bir formatta değil")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}

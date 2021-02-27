using System.ComponentModel.DataAnnotations;

namespace TurkAzn.Web.Models.Kullanici
{
    public class KayitViewModel
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
        [Required(ErrorMessage = "Hepsinin doldurulması zorunludur")]
        [StringLength(100, ErrorMessage = "Girilen şifre en az 6 karakter içermelidir.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrarı")]
        [Compare("Sifre", ErrorMessage = "Girilen 2 şifre birbiriyle uyuşmuyor.")]
        public string SifreTekrar { get; set; }
        //[Required(ErrorMessage = "Hepsinin doldurulması zorunludur")]
        //public bool FreelancerOL { get; set; }
        //[Required(ErrorMessage = "Hepsinin doldurulması zorunludur")]
        //[Display(Name = "Kullanıcı Adı")]
        //public string KullaniciAdi { get; set; }
    }
}

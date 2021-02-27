using System.ComponentModel.DataAnnotations;

namespace TurkAzn.Web.Models
{
    public class AdresEkleViewModel
    {

        [Required(ErrorMessage = "Adresinize ad vermeyi unutmayın")]
        [Display(Name = "Adres Ad")]
        public string AdresName { get; set; }
        [Required(ErrorMessage = "İl Seçiniz")]
        [Display(Name = "İl")]
        public string il { get; set; }
        [Required(ErrorMessage = "İlçe seçiniz")]
        [Display(Name = "İlçe")]
        public string ilce { get; set; }
        [Required(ErrorMessage = "Adresinizi yazınız")]
        [Display(Name = "Adres")]
        public string description { get; set; }
        [Required(ErrorMessage = "Alıcı ad soyad doldurunuz")]
        [Display(Name = "Alıcı Adı ve Soyadı")]
        public string namesurname { get; set; }
        [Required(ErrorMessage = "Posta kodu boş bırakılamaz")]
        [Display(Name = "Posta kodu")]
        public string zipcode { get; set; }
    }
}
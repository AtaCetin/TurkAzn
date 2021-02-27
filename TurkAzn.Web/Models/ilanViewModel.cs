using System.ComponentModel.DataAnnotations;

namespace TurkAzn.Web.Models
{
    public class ilanViewModel
    {
        [Required]
        public string ilanBaslik { get; set; }
        [Required]
        [DataType( DataType.Currency)]
        public int ilanUcret { get; set; }
        [Required]
        public string Aciklama { get; set; }
        [Required]
        public string kisa_ad { get; set; }
        [Required]
        public int KategoriID { get; set; }
    }
}

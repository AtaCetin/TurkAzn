namespace TurkAzn.Web.Models
{
    public class SepetIndexModel
    {
        public TurkAzn.Web.Models.ShoppingCart ShoppingCart { get; set; }
        public decimal ShoppingCartTotal { get; set; }
        public string ReturnUrl { get; set; }
    }
}
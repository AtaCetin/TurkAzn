using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurkAzn.Web.Data;
using TurkAzn.Web.Models;

namespace TurkAzn.Web.Controllers
{
   
    public class SepetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCart _shoppingCart;

        public SepetController(ApplicationDbContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Index(bool isValidAmount = true, string returnUrl = "/")
        {
            _shoppingCart.GetShoppingCartItems();

            var model = new SepetIndexModel()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
                ReturnUrl = returnUrl
            };

            if (!isValidAmount)
            {
                ViewBag.InvalidAmountText = "*There were not enough items in stock to add*";
            }

            return View("Index", model);
        }

        public IActionResult _SepetUrunSayisi()
        {
            ViewBag.SepetSayisi = _shoppingCart.GetShoppingCartItems().Count();
            return PartialView();
        }



        [HttpGet]
        [Route("/Sepet/Ekle/{id}/{returnUrl?}")]
        public IActionResult Ekle(int id, int? amount = 1, string returnUrl=null )
        {
            var urun = _context.Urunler.Include(x=>x.UrunSahibi).FirstOrDefault(x => x.UrunID==id);
            //returnUrl = returnUrl.Replace("%2F", "/");
            returnUrl = "/sepet/index";
            bool isValidAmount = false;
            if (urun != null)
            {
                isValidAmount = _shoppingCart.AddToCart(urun, amount.Value);
            }

            return Index(isValidAmount, returnUrl);
        }

        [HttpGet]
        public IActionResult Amount(int urunID, int Amount)
        {
            var urun = _context.Urunler.Find(urunID);
            var returnUrl = "/sepet/index";
            bool isValidAmount = false;
            
            if (urun != null)
            {
                _shoppingCart.RemoveFromCart(urun);
                isValidAmount = _shoppingCart.AddToCart(urun, Amount);
            }

            return Index(isValidAmount, returnUrl);
        }
        
        
        public IActionResult Kaldir(int urunID)
        {
            var food = _context.Urunler.Find(urunID);
            if (food != null)
            {
                _shoppingCart.RemoveFromCart(food);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Back(string returnUrl="/")
        {
            return Redirect(returnUrl);
        }
    }
}
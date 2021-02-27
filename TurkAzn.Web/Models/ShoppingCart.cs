using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurkAzn.Data;
using TurkAzn.Web.Data;

namespace TurkAzn.Web.Models
{
	public class ShoppingCart
	{
		private readonly ApplicationDbContext _context;

		public ShoppingCart(ApplicationDbContext context)
		{
			_context = context;
		}

		public string Id { get; set; }
		public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }

		public static ShoppingCart GetCart(IServiceProvider services)
		{
			ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
			var context = services.GetService<ApplicationDbContext>();
			string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

			session.SetString("CartId", cartId);
			return new ShoppingCart(context) { Id = cartId };
		}

		public bool AddToCart(Urun urun, int amount)
		{
			if(urun.StokSayisi == 0 || amount == 0)
			{
				return false;
			}
			
			var shoppingCartItem = _context.SepetEsyasi.SingleOrDefault(
				s => s.Urun.UrunID == urun.UrunID && s.ShoppingCartId == Id);
            var isValidAmount = true;
			if (shoppingCartItem == null)
			{
                if (amount > urun.StokSayisi)
                {
                    isValidAmount = false;
                }
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = Id,
                    Urun = urun,
                    Amount = Math.Min(urun.StokSayisi, amount)
				};
				_context.SepetEsyasi.Add(shoppingCartItem);
			}
			else
			{
                if(urun.StokSayisi - shoppingCartItem.Amount - amount >= 0)
                {
                    shoppingCartItem.Amount +=  amount;
                }
                else
                {
					shoppingCartItem.Amount += (urun.StokSayisi - shoppingCartItem.Amount);
                    isValidAmount = false;
                }
            }


			_context.SaveChanges();
            return isValidAmount;
		}

		public int RemoveFromCart(Urun urun)
		{
			var shoppingCartItem = _context.SepetEsyasi.SingleOrDefault(
				s => s.Urun.UrunID == urun.UrunID && s.ShoppingCartId == Id);
			int localAmount = 0;
			if (shoppingCartItem != null)
			{
				if (shoppingCartItem.Amount > 1)
				{
					shoppingCartItem.Amount--;
					localAmount = shoppingCartItem.Amount;
				}
				else
				{
					_context.SepetEsyasi.Remove(shoppingCartItem);
				}
			}

			_context.SaveChanges();
			return localAmount;
		}

		public IEnumerable<ShoppingCartItem> GetShoppingCartItems()
		{
			return ShoppingCartItems ??
				   (ShoppingCartItems = _context.SepetEsyasi.Where(c => c.ShoppingCartId == Id)
					   .Include(s => s.Urun).ThenInclude(x=>x.UrunSahibi));
		}

		public void ClearCart()
		{
			var cartItems = _context
				.SepetEsyasi
				.Where(cart => cart.ShoppingCartId == Id);

			_context.SepetEsyasi.RemoveRange(cartItems);
			_context.SaveChanges();
		}

		public decimal GetShoppingCartTotal()
		{
			return _context.SepetEsyasi.Where(c => c.ShoppingCartId == Id)
				.Select(c => c.Urun.UrunFiyat * c.Amount).Sum();
		}

	}
}

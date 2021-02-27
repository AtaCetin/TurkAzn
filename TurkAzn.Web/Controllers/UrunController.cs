using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ImageMagick;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.IdentityModel.Xml;
using Newtonsoft.Json;
using TurkAzn.Data;
using TurkAzn.Data.Kimlik;
using TurkAzn.Web.Data;
using TurkAzn.Web.Models;
using X.PagedList;

namespace DevFreelancer.Web.Controllers
{
    public class UrunController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Kullanici> _userManager;
        private readonly IFileProvider fileProvider;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly Options options;
        private IHttpContextAccessor _accessor;
        private readonly int komisyon;
        private readonly ShoppingCart _shoppingCart;

        public UrunController(ApplicationDbContext context, UserManager<Kullanici> userManager, IFileProvider fileprovider, IHostingEnvironment env, IHttpContextAccessor accessor, ShoppingCart shoppingCard)
        {
            _shoppingCart = shoppingCard;
            _userManager = userManager;
            _context = context;
            fileProvider = fileprovider;
            hostingEnvironment = env;
            _accessor = accessor;

            var ayarlar = _context.SiteAyari.FirstOrDefault();
            Options Options = new Options();
            komisyon = ayarlar.komisyon;
            Options.ApiKey = ayarlar.IyzicoApiKey;
            Options.BaseUrl = ayarlar.IyzicoBaseURL;
            Options.SecretKey = ayarlar.IyzicoApiSecret;
            options = Options;
        }
        [Route("urun/Listele/{kategoriadi}")]
        public async Task<IActionResult> Listele(string kategoriadi = "", [FromQuery] int? sayfa = 1)
        {
            var sayfaNum = sayfa ?? 1;

            if (kategoriadi.Length > 0)
            {

                var kategori = await _context.Kategoriler.Include(x=>x.KategorininAlt).Include(x => x.kategoridekiUrunler).ThenInclude(x => x.UrunSahibi).Include(x => x.kategoridekiUrunler).ThenInclude(x => x.UrunResimleri).Where(x => x.kisaad == kategoriadi).FirstOrDefaultAsync();

                if (kategori==null)
                {
                    return NotFound();
                }

                ViewBag.Kategori = kategori.KategoriAd;

                if (kategori.KategorininAlt.Count > 0)
                {
                    await _context.Entry(kategori).Collection(x => x.KategorininAlt).Query()
                        .Include(x => x.kategoridekiUrunler).ThenInclude(x => x.UrunSahibi)
                        .Include(x => x.kategoridekiUrunler).ThenInclude(x => x.UrunResimleri).LoadAsync();
                    var liste = kategori.kategoridekiUrunler;

                    foreach (Kategori item in kategori.KategorininAlt)
                    {
                        foreach (var item2 in item.kategoridekiUrunler)
                        {
                            liste.Add(item2);
                        }
                    }

                    liste.OrderBy(x => x.BegeniNumarasi);
                    var ilanlar = liste.AsQueryable().ToPagedList(sayfaNum, 20);
                    ViewBag.UstKategori = kategori;
                    return View(ilanlar);
                }
                else
                {
                    var ustkategori = await _context.Kategoriler.Include(x => x.KategorininAlt).ThenInclude(x => x.kategoridekiUrunler).Where(x => x.KategorininAlt.Contains(kategori))
                        .FirstOrDefaultAsync();
                    var ilanlar = kategori.kategoridekiUrunler.AsQueryable().OrderBy(x=>x.BegeniNumarasi).ToPagedList(sayfaNum, 20);
                    ViewBag.UstKategori = ustkategori;
                    return View(ilanlar);
                }

            }
            return NotFound();
        }

        public async Task<IActionResult> Ara([FromQuery] string aranankelime = "")
        {
            ViewBag.arananKelime = aranankelime;

            if (aranankelime.Length > 0)
            {
                var aramaKelimeleri = aranankelime.ToUpper().Split(' ').ToList();
                var sonuc = new List<Urun>();

                foreach (var item in aramaKelimeleri)
                {
                    var ilanlar = await _context.Urunler.Include(x => x.UrunSahibi).Include(x => x.UrunResimleri)
                        .Where(x => x.UrunAciklama.ToUpper().Contains(item) || x.UrunBaslik.ToUpper().Contains(item)).ToListAsync();
                    foreach (var item2 in ilanlar)
                    {
                        if (!sonuc.Contains(item2))
                        {
                            sonuc.Add(item2);
                        }
                    }
                }

                sonuc.OrderBy(x => x.BegeniNumarasi);

                //var ilanlar = await _context.ilanlar.AsNoTracking().Include(x => x.UrunSahibi)
                //    .Include(x => x.UrunResimleri)
                //    .ToListAsync();
                //var sonuc = await ilanlar.Where(data => aramaKelimeleri.Any(x => data.ilanBaslik.Contains(x)) ||
                //                                  aramaKelimeleri.Any(x => data.ilanAciklama.Contains(x))).ToListAsync();



                ViewBag.Kullanicilar = await _context.Kullanicilar
                    .Include(x => x.Avatar)
                    .Include(x => x.MarketProfili)
                    .Where(x => x.MarketHesabimi == true)
                    .Where(x =>
                        x.UserName.Contains(aranankelime) || x.Ad.Contains(aranankelime) ||
                        x.Soyad.Contains(aranankelime))
                    .ToListAsync();


                int sonucsayisi = sonuc.Count;
                sonucsayisi += ViewBag.Kullanicilar.Count;

                ViewBag.sonuc = sonucsayisi;
                return View(sonuc);
            }
            else
            {
                return BadRequest();
            }

        }



        [HttpGet("urun/goster/{kisaad}/{Urunkisaad}")]
        public async Task<IActionResult> urunGoster(string kisaad, string Urunkisaad)
        {
            var market = await _context.Marketler.AsNoTracking().Where(x => x.kisaAd == kisaad).FirstOrDefaultAsync();
            var urun = await _context.Urunler.AsNoTracking().Where(x => x.UrunSahibi == market).Include(i => i.UrunResimleri).Include(x => x.UrunYorumlari).ThenInclude(x => x.YorumuYazan).ThenInclude(x => x.Avatar).Where(y => y.Urunkisaad == Urunkisaad).FirstOrDefaultAsync();

            if (urun == null || kisaad == null)
            {
                return NotFound();
            }

            urun.UrunSahibi = market;
            if (urun.UrunAktifmi && market != null && urun != null)
            {
                return View(urun);
            }
            else if (!urun.UrunAktifmi)
            {
                return RedirectToAction("AktifDegil");
            }
            else
            {
                return NotFound();
            }

        }

        public IActionResult AktifDegil()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Ekle()
        {
            var kullanici = await kullaniciGetir(User.Identity.Name);

            if (!kullanici.MarketHesabimi)
            {
                return RedirectToAction("MarketKayit", "Hesap");
            }

            ViewBag.Kategori = new SelectList(_context.Kategoriler
                .Select(x =>
                    new SelectListItem
                    {
                        Value = x.KategoriID.ToString(),
                        Text = x.KategoriAd
                    }), "Value", "Text");



            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Ekle(ilanViewModel yeniIlan, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var kullanici = await _userManager.GetUserAsync(User);
                var market = await _context.Marketler.Include(x => x.Urunler).Where(x => x.MarketID == kullanici.Id).FirstOrDefaultAsync();

                if (market.Urunler.Where(x => x.Urunkisaad == yeniIlan.kisa_ad).Any())
                {
                    ModelState.AddModelError(string.Empty, "Ayni kisa ada sahip ikinci bir ilan açamazsınız");
                    return View(yeniIlan);
                }

                var gelenkategori = await _context.Kategoriler.FindAsync(yeniIlan.KategoriID);

                Urun i = new Urun()
                {
                    UrunAciklama = yeniIlan.Aciklama,
                    UrunAktifmi = false,
                    UrunDegismeZamani = DateTime.Now,
                    UrunYaratilmaZamani = DateTime.Now,
                    UrunBaslik = yeniIlan.ilanBaslik,
                    UrunFiyat = yeniIlan.ilanUcret,
                    Urunkisaad = yeniIlan.kisa_ad,
                    BegeniNumarasi = 0,
                    UrunSahibi = market,
                    KategoriAdi = gelenkategori.KategoriAd
                };

                i.UrunResimleri = new List<Resim>();

                await _context.Urunler.AddAsync(i);
                await _context.SaveChangesAsync();
                gelenkategori.kategoridekiUrunler.Add(i);
                _context.Kategoriler.Update(gelenkategori);
                market.Urunler.Add(i);
                _context.Update(market);
                await _context.SaveChangesAsync();
                foreach (IFormFile file in files)
                {
                    if (file != null || file.Length != 0)
                    {
                        FileInfo fi = new FileInfo(file.FileName);
                        var newFilename = i.UrunID + "_" + String.Format("{0:d}", (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                        var webPath = hostingEnvironment.WebRootPath;
                        string path2 = this.hostingEnvironment.WebRootPath + "\\Resimler\\" + kullanici.UserName;
                        if (!Directory.Exists(path2))
                            Directory.CreateDirectory(path2);
                        var path = Path.Combine("", webPath + @"\Resimler\" + kullanici.UserName + @"\" + newFilename);
                        var pathToSave = @"/Resimler/" + kullanici.UserName + @"/" + newFilename;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        Resim r = new Resim()
                        { ResimYolu = pathToSave, ResimAciklama = file.FileName };

                        await _context.Resimler.AddAsync(r);
                        i.UrunResimleri.Add(r);
                        _context.Urunler.Update(i);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Basarili", "Urun", new { ilanID = i.UrunID });


            }
            ViewBag.Kategori = new SelectList(_context.Kategoriler
                .Select(x =>
                    new SelectListItem
                    {
                        Value = x.KategoriID.ToString(),
                        Text = x.KategoriAd
                    }), "Value", "Text");



            return View();
        }
        public async Task<IActionResult> Basarili(int ilanID)
        {
            if (ilanID > 0)
            {
                var gelenilan = await _context.Urunler.FindAsync(ilanID);
                return View(gelenilan);
            }
            else
            {
                return NotFound();
            }
        }
       
      
        [Authorize]
        [HttpGet("urun/satinal/{id}")]
        public async Task<IActionResult> Satin_al(int id)
        {
            var kullanici = await kullaniciGetir(User.Identity.Name);
            var adres = kullanici.Adresler.Where(x => x.adresID == id).FirstOrDefault();

            if (adres!=null)
            {
                
                
                _shoppingCart.GetShoppingCartItems();

                var model = new SepetIndexModel()
                {
                    ShoppingCart = _shoppingCart,
                    ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
                    ReturnUrl = "returnUrl"
                };
                
                if (_shoppingCart.GetShoppingCartTotal() == 0)
                {
                    ModelState.AddModelError("", "Sepetinize birşeyler ekleyin");
                    return RedirectToAction("Index", "Sepet");
                }
                

                SatinAlmaViewModel sAvm = new SatinAlmaViewModel();
                sAvm.GelenSepet = model;
                sAvm.SecilenAdres = adres;
                sAvm.GelenKullanici = kullanici;

                return View(sAvm);
            }

            return NotFound();

        }
        [HttpPost("urun/satinal/{id}")]
        public async Task<IActionResult> Satin_al(string id)
        {
            var kullanici = await kullaniciGetir(User.Identity.Name);
            var adresID = Convert.ToInt32(id);
            var adres = kullanici.Adresler.Where(x => x.adresID == adresID).FirstOrDefault();

            if (adres!=null)
            {
                
                
                _shoppingCart.GetShoppingCartItems();

                var model = new SepetIndexModel()
                {
                    ShoppingCart = _shoppingCart,
                    ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal(),
                    ReturnUrl = "returnUrl"
                };
                
                if (_shoppingCart.GetShoppingCartTotal() == 0)
                {
                    ModelState.AddModelError("", "Sepetinize birşeyler ekleyin");
                    return RedirectToAction("Index", "Sepet");
                }

                SatinAlmaViewModel sAvm = new SatinAlmaViewModel();
                sAvm.GelenSepet = model;
                sAvm.SecilenAdres = adres;
                sAvm.GelenKullanici = kullanici;
                
                var baseUrl = UriHelper.GetDisplayUrl(this.Request);
                
                 var list = baseUrl.Split('/').ToList();
                 var url = list[0] + "//" + list[2] + "/Urun/ode";
            
                 CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
                 request.Locale = Locale.TR.ToString();
                 request.Price = model.ShoppingCartTotal.ToString("#.#");
                 request.PaidPrice = model.ShoppingCartTotal.ToString("#.#");
                 request.Currency = Currency.TRY.ToString();
                 request.BasketId = _shoppingCart.Id;
                 request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
                 request.CallbackUrl = url;
            
                 List<int> enabledInstallments = new List<int>();
                 enabledInstallments.Add(2);
                 enabledInstallments.Add(3);
                 enabledInstallments.Add(6);
                 enabledInstallments.Add(9);
                 request.EnabledInstallments = enabledInstallments;
            
                 Buyer buyer = new Buyer();
                 buyer.Id = kullanici.Id;
                 buyer.Name = kullanici.Ad;
                 buyer.Name.ToUpperInvariant();
                 buyer.Surname = kullanici.Soyad;
                 buyer.GsmNumber = kullanici.Telefon_Numarasi;
                 buyer.Email = kullanici.Email;
                 buyer.IdentityNumber = kullanici.Kimlik;
                 buyer.LastLoginDate = kullanici.EnSonOturumAcma.ToString("u");
                 buyer.RegistrationDate = kullanici.OlusturulmaTarihi.ToString("u");
                 buyer.RegistrationAddress = adres.description;
                 buyer.Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                 buyer.City = adres.city;
                 buyer.Country = "Azerbeycan";
                 buyer.ZipCode = adres.zipcode;
                 request.Buyer = buyer;
            
                 Address shippingAddress = new Address();
                 shippingAddress.ContactName = adres.namesurname;
                 shippingAddress.City = adres.city;
                 shippingAddress.Country = "Azerbeycan";
                 shippingAddress.Description = adres.description;
                 shippingAddress.ZipCode = adres.zipcode.ToString();
                 request.ShippingAddress = shippingAddress;
            
                 Address billingAddress = new Address();
                 billingAddress.ContactName = adres.namesurname;
                 billingAddress.City = adres.city;
                 billingAddress.Country = "Azerbeycan";
                 billingAddress.Description = adres.description;
                 billingAddress.ZipCode = adres.zipcode.ToString();
                 request.BillingAddress = billingAddress;
            
                 List<BasketItem> basketItems = new List<BasketItem>();
                 foreach (var satilanUrunler in _shoppingCart.ShoppingCartItems)
                 {
                     for (int i = 0; i < satilanUrunler.Amount; i++)
                     {
                         decimal bizimalacagimiz = (komisyon * satilanUrunler.Urun.UrunFiyat / 100);
            
                         decimal altuyenin = satilanUrunler.Urun.UrunFiyat - bizimalacagimiz;
                         BasketItem firstBasketItem = new BasketItem();
                         firstBasketItem.Id = satilanUrunler.Urun.UrunID.ToString();
                         firstBasketItem.Name = satilanUrunler.Urun.Urunkisaad;
                         firstBasketItem.Category1 = satilanUrunler.Urun.KategoriAdi;
                         firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                         firstBasketItem.Price = satilanUrunler.Urun.UrunFiyat.ToString("#.#");
                         firstBasketItem.SubMerchantPrice = altuyenin.ToString("#.#");
                         firstBasketItem.SubMerchantKey = satilanUrunler.Urun.UrunSahibi.SubMerchantKey;
                         basketItems.Add(firstBasketItem);
                     }
                 }

                 TempData["adres"] = adresID;
            
                 request.BasketItems = basketItems;
            
            
                 CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
                 
                 ViewBag.Java = checkoutFormInitialize.CheckoutFormContent;
                 ViewBag.hata = checkoutFormInitialize.ErrorMessage;

                return View(sAvm);
                //return Redirect(checkoutFormInitialize.PaymentPageUrl);
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> ode(string token)
        {
            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest {Token = token};

            CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);
            if (checkoutForm.Status == "success")
            {
                int adresID = Convert.ToInt32(TempData["adres"]);
                var adres = await _context.Adresler.FirstOrDefaultAsync(x => x.adresID == adresID);
                var satisid = checkoutForm.BasketId;
                _shoppingCart.GetShoppingCartItems();
                var kullanici = await kullaniciGetir(User.Identity.Name);

                List<Satis> satislar = new List<Satis>();
                
                foreach (var gelenurun in _shoppingCart.ShoppingCartItems)
                {
                    Durum d = new Durum();
                    d.DurumTarihi = DateTime.Now;
                    d.durumIcerik = "Ödeme Yapıldı";
                    await _context.Durumlar.AddAsync(d);
                    
                    
                    await _context.SaveChangesAsync();
                    
                    Satis s = new Satis();
                    s.Alan_Kullanici = kullanici;
                    s.verilenAdres = adres;
                    s.Onaylandimi = false;
                    s.odenenPara = gelenurun.Amount * gelenurun.Urun.UrunFiyat;
                    s.miktar = gelenurun.Amount;
                    s.AlinmaTarihi = DateTime.Now;
                    s.iyzicoToken = checkoutForm.PaymentItems.FirstOrDefault().PaymentTransactionId;
                    s.odendiMi = true;
                    s.Satan_Market = gelenurun.Urun.UrunSahibi;
                    s.SatilanUrun = gelenurun.Urun;
                    s.SepetID = _shoppingCart.Id;
                    
                    s.SatisDurumu.Add(d);
            
                    _context.Satislar.Add(s);
                    
                    var urun = await _context.Urunler.FindAsync(s.SatilanUrun.UrunID);
                    urun.SatilmaSayisi++;
                    urun.StokSayisi--;
                    _context.Urunler.Update(urun);
                    satislar.Add(s);
                }
                await _context.SaveChangesAsync();
                _shoppingCart.ClearCart();
                ViewBag.Sonuc = true;
                TempData["adres"] = null;

                return View(satislar);
            }
            else
            {
                
                ViewBag.Hata = checkoutForm.ErrorMessage;
                ViewBag.Sonuc = false;
                return View();
            }
        }
        [Authorize]
        [Route("ilan/Onayla/{satisID}")]
        public async Task<IActionResult> Onayla(int satisID)
        {
            var gelenkullanici = await kullaniciGetir(User.Identity.Name);
            var gelenSAtis = await _context.Satislar.Include(x => x.Alan_Kullanici).Include(x => x.Satan_Market).Include(x => x.SatilanUrun).Where(x => x.SatisID == satisID).FirstOrDefaultAsync();
            if (gelenSAtis.Alan_Kullanici.Id == gelenkullanici.Id)
            {
                onaylaViewModel ovm = new onaylaViewModel();
                ovm.Satis = gelenSAtis;
                ovm.kabulEttimi = false;
                return View(ovm);
            }
            else
            {
                return NotFound();
            }

        }
        [Authorize]
        [HttpPost]
        [Route("ilan/Onayla/{satisID}")]
        public async Task<IActionResult> Onayla(int satisID, onaylaViewModel ovm)
        {
            var gelenkullanici = await kullaniciGetir(User.Identity.Name);
            var gelenSAtis = await _context.Satislar.Include(x => x.Alan_Kullanici).Include(x => x.Satan_Market).Include(x => x.SatilanUrun).Where(x => x.SatisID == satisID).Include(x => x.SatisDurumu).FirstOrDefaultAsync();
            if (gelenSAtis.Alan_Kullanici.Id == gelenkullanici.Id)
            {
                if (ovm.kabulEttimi)
                {
                    CreateApprovalRequest request = new CreateApprovalRequest();
                    request.Locale = Locale.TR.ToString();
                    request.PaymentTransactionId = gelenSAtis.iyzicoToken;

                    Approval approval = Approval.Create(request, options);


                    if (approval.Status == "success")
                    {
                        Durum d = new Durum();
                        d.DurumTarihi = DateTime.Now;
                        d.durumIcerik = "Kullanıcı tarafından onaylandı";

                        await _context.Durumlar.AddAsync(d);
                        await _context.SaveChangesAsync();

                        gelenSAtis.SatisDurumu.Add(d);
                        gelenSAtis.Onaylandimi = true;
                        gelenSAtis.OnaylanmaTarihi = DateTime.Now;
                        _context.Satislar.Update(gelenSAtis);
                        await _context.SaveChangesAsync();
                        ViewBag.Durum = true;

                    }
                    else
                    {
                        ViewBag.Durum = false;
                        ViewBag.Hata = approval.ErrorMessage;
                    }

                    ovm.Satis = gelenSAtis;
                    return View(ovm);
                }
                else
                {
                    return RedirectToAction("Index", "Anasayfa");
                }
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPost]
        public async Task<IActionResult> YorumYap(string fKullaniciAdi, string yorumIcerik, int? puan, int? yorumID, int ilanID, string ilanKisaAd)
        {
            if (ModelState.IsValid)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    ModelState.AddModelError("", "Oturum Açın");
                    return RedirectToAction("ilanGoster", new { kullaniciadi = fKullaniciAdi, ilankisaad = ilanKisaAd });
                }
                if (fKullaniciAdi == User.Identity.Name)
                {
                    ModelState.AddModelError("", "KENDINIZE YORUM YAPMANIZ YASAKTIR");
                    return RedirectToAction("ilanGoster", new { kullaniciadi = fKullaniciAdi, ilankisaad = ilanKisaAd });
                }
                if (yorumID.HasValue)
                {
                    var yorum = await _context.Yorumlar.FindAsync(yorumID);
                    var yorumYazan = await _context.Kullanicilar.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
                    Yorum yeni = new Yorum() { YorumunIcerigi = yorumIcerik, YorumuYazan = yorumYazan, YorumDegistirilme = DateTime.Now, YorumSilindimi = false, YorumYazilma = DateTime.Now, };
                    await _context.Yorumlar.AddAsync(yeni);
                    await _context.SaveChangesAsync();
                    yorum.AltCevap = yeni;
                    _context.Yorumlar.Update(yorum);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ilanGoster", new { kullaniciadi = fKullaniciAdi, ilankisaad = ilanKisaAd });
                }
                else if (puan.HasValue)
                {
                    var gelenilan = await _context.Urunler.Include(x => x.UrunYorumlari).Where(x => x.UrunID == ilanID).FirstOrDefaultAsync();
                    var yorumYazan = await _context.Kullanicilar.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
                    Yorum yeni = new Yorum() { YorumunIcerigi = yorumIcerik, YorumuYazan = yorumYazan, VerilenPuan = puan.Value, YorumDegistirilme = DateTime.Now, YorumYazilma = DateTime.Now, YorumSilindimi = false };
                    await _context.Yorumlar.AddAsync(yeni);
                    await _context.SaveChangesAsync();
                    if (gelenilan.UrunYorumlari.Any())
                    {
                        int begenisayisi = gelenilan.UrunYorumlari.Sum(x => x.VerilenPuan);
                        gelenilan.BegeniNumarasi = (begenisayisi + puan.Value) / gelenilan.UrunYorumlari.Count();
                    }
                    else
                    {
                        gelenilan.BegeniNumarasi = puan.Value;
                    }
                    gelenilan.UrunYorumlari.Add(yeni);
                    _context.Urunler.Update(gelenilan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ilanGoster", new { kullaniciadi = fKullaniciAdi, ilankisaad = ilanKisaAd });
                }
                else
                {
                    ModelState.AddModelError("", "Puan girmeyi unutmuşsunuz");
                    return RedirectToAction("ilanGoster", new { kullaniciadi = fKullaniciAdi, ilankisaad = ilanKisaAd });
                }
            }


            return RedirectToAction("ilanGoster", new { kullaniciadi = fKullaniciAdi, ilankisaad = ilanKisaAd });
        }

      

        public async Task<Kullanici> kullaniciGetir(string kullaniciAdi) => await _context.Kullanicilar.Where(x => x.UserName == kullaniciAdi).Include(x => x.Mesajlar).Include(x=>x.Adresler).FirstOrDefaultAsync();



        public async Task<IActionResult> Kesfet()
        {
            ViewBag.Kategoriler = await _context.Kategoriler.Include(x => x.kategoridekiUrunler).Take(10).ToListAsync();
            return View();
        }
    }
}

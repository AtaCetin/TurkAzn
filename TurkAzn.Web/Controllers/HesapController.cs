using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ImageMagick;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TurkAzn.Data;
using TurkAzn.Data.Kimlik;
using TurkAzn.Web.Data;
using TurkAzn.Web.Models;
using TurkAzn.Web.Models.Kullanici;

namespace TurkAzn.Web.Controllers
{
    [Authorize]
    public class HesapController : Controller
    {
        private readonly UserManager<Kullanici> _userManager;
        private readonly SignInManager<Kullanici> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ApplicationDbContext _context;
        public HesapController(UserManager<Kullanici> userManager,
            SignInManager<Kullanici> signInManager,
            IEmailSender emailSender,
            ILogger<HesapController> logger,
            IHostingEnvironment env,
            RoleManager<Rol> roleManager,
            ApplicationDbContext context)
        {
            _context = context;
            hostingEnvironment = env;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }



        [AllowAnonymous]
        
        public IActionResult GirisYap()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GirisYap(GirisViewModel GirisViewModel, string returnUrl)
        {
            if (returnUrl == null)
            {
                returnUrl = "/Hesap/Panel";
            }

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(GirisViewModel.Email,
                    GirisViewModel.Sifre, GirisViewModel.BeniHatirla, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var gelen = await kullaniciGetir(GirisViewModel.Email);
                    await EnSonOturumAcma(gelen);
                    return Redirect(returnUrl);
                }

                if (result.IsNotAllowed)
                {
                    var gelen = await kullaniciGetir(GirisViewModel.Email);
                    ModelState.AddModelError(string.Empty, "E-mail'inizi doğrulamanız gerekiyor");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(gelen);
                    EmailYolla(gelen.Email, "Email dogrulama linkiniz https://turkazn.atacetin.com/hesap/dogrula?email=" + gelen.Email + "&kod=" + Base64UrlEncode(Encoding.ASCII.GetBytes(code)), "E-mail Doğrulama Linkiniz");
                    return View(GirisViewModel);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(HesapKilitli));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "E-mail veya Şifre yanlış");
                    return View(GirisViewModel);
                }
            }
            ModelState.AddModelError(string.Empty, "Formu doldurmayı unutmayın");
            return View();
        }


        public async Task<IActionResult> Duzenle() => View(await kullaniciGetir(User.Identity.Name));

        [HttpPost]
        public async Task<IActionResult> Duzenle(string Ad, string Soyad, string Telefon_Numarasi)
        {
            var gelen = await kullaniciGetir(User.Identity.Name);
            if (ModelState.IsValid)
            {
                
                gelen.Ad = Ad;
                gelen.Soyad = Soyad;
                gelen.Telefon_Numarasi = Telefon_Numarasi;
                return RedirectToAction("Profil");
            }

            return View(gelen);
        }

        private async Task EnSonOturumAcma(Kullanici gelen)
        {
            gelen.EnSonOturumAcma = DateTime.Now;
            _context.Kullanicilar.Update(gelen);
            await _context.SaveChangesAsync();
            
        }

        [AllowAnonymous]
        public IActionResult KayitOl() => View();


        public IActionResult AvatarYukle() => View();

        [HttpPost]
        public async Task<IActionResult> AvatarYukle(IFormFile file)
        {
            var gelen = await kullaniciGetir(User.Identity.Name);

            if (file != null || file.Length != 0)
            {
                FileInfo fi = new FileInfo(file.FileName);
                var newFilename = gelen.Ad + "_" + String.Format("{0:d}", (DateTime.Now.Ticks / 10) % 100000000) +
                                  fi.Extension;
                var webPath = hostingEnvironment.WebRootPath;
                string path2 = this.hostingEnvironment.WebRootPath + "\\Resimler\\" + gelen.UserName;
                if (!Directory.Exists(path2))
                    Directory.CreateDirectory(path2);
                var path = Path.Combine("", webPath + @"\Resimler\" + gelen.UserName + @"\" + newFilename);
                var pathToSave = @"/Resimler/" + gelen.UserName + @"\" + newFilename;
                //using (var stream = new FileStream(path, FileMode.Create))
                //{
                //    await file.CopyToAsync(stream);
                //}

                const int size = 150;
                const int quality = 75;

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();


                    using (var image = new MagickImage(fileBytes))
                    {
                        image.Resize(size, size);
                        image.Strip();
                        image.Quality = quality;
                        image.Write(path);
                    }

                }



                Resim r = new Resim()
                { ResimYolu = pathToSave, ResimAciklama = file.FileName };

                await _context.Resimler.AddAsync(r);
                gelen.Avatar = r;
                _context.Kullanicilar.Update(gelen);
                await _context.SaveChangesAsync();
                return RedirectToAction("Profil");
            }

            return View();
        }

        public IActionResult SifreDegistir() => View();
        [HttpPost]
        public async Task<IActionResult> SifreDegistir(string suankiSifre, string Yenisifre)
        {
            var gelen = await kullaniciGetir(User.Identity.Name);
            IdentityResult x = await _userManager.ChangePasswordAsync(gelen, suankiSifre, Yenisifre);
            if (x.Succeeded)
            {
                return RedirectToAction("Profil");
            }
            else
            {

                return View();
            }
        }


        public void EmailYolla(string email, string mesaj, string konu)
        {
            try
            {
                SmtpClient client = new SmtpClient("104.238.176.209", 25)
                {
                    EnableSsl = false,
                };
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("admin", "444466");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("noreply@turkazn.com");
                mailMessage.To.Add(email);
                mailMessage.Body = mesaj;
                mailMessage.Subject = konu;
                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                _logger.LogInformation("E-mail yollanamadı.");
                _logger.LogInformation(e.Message);
            }
        }
        [Route("hesap/profil")]
        [Route("profil/{kullaniciadi}")]
        public async Task<IActionResult> Profil(string kullaniciadi)
        {
            var gelenkullanici = await _context.Kullanicilar.Where(x => x.UserName == kullaniciadi).Include(x => x.Avatar).FirstOrDefaultAsync();
            if (gelenkullanici != null)
            {


                if (gelenkullanici.MarketHesabimi)
                {
                    _context.Entry(gelenkullanici)
                           .Reference(b => b.MarketProfili)
                           .Load();
                    return View(gelenkullanici);
                }
                else
                {
                    return View(gelenkullanici);
                }
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    gelenkullanici = await _context.Kullanicilar.Where(x => x.UserName == User.Identity.Name).Include(x => x.Avatar).FirstOrDefaultAsync();
                    await EnSonOturumAcma(gelenkullanici);
                    return View(gelenkullanici);
                }
                return NotFound();

            }
        }

        public async Task<IActionResult> CikisYap()
        {
            await _signInManager.SignOutAsync();
            return View();
        }


        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> KayitOl(KayitViewModel kayitOlanKullanici, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new Kullanici { UserName = kayitOlanKullanici.Email, Email = kayitOlanKullanici.Email, Ad = kayitOlanKullanici.Ad, DegistirilmeTarihi = DateTime.Now, Telefon_Numarasi = kayitOlanKullanici.TelefonNumarasi, AktifMi = true, EnSonOturumAcma = DateTime.Now, OlusturulmaTarihi = DateTime.Now, Soyad = kayitOlanKullanici.Soyad };
                var result = await _userManager.CreateAsync(user, kayitOlanKullanici.Sifre);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Yeni kullanıcı şifreyle yaratıldı.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    // await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
                    EmailYolla(user.Email, "Email dogrulama linkiniz https://turkazn.atacetin.com/hesap/dogrula?email=" + user.Email + "&kod=" + Base64UrlEncode(Encoding.ASCII.GetBytes(code)), "E-mail Doğrulama Linkiniz");


                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Basarili");
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Beklenmiyen bir hata oluştu");
                    return View(kayitOlanKullanici);
                }

            }
            return View(kayitOlanKullanici);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Dogrula(string kod, string email)
        {
            var gelenkod = Encoding.ASCII.GetString(Base64UrlDecode(kod));
            var kul = await _context.Kullanicilar.Where(x => x.Email == email).FirstOrDefaultAsync();
            var cevap = await _userManager.ConfirmEmailAsync(kul, gelenkod);
            if (cevap.Succeeded)
            {
                return View();
            }
            return NotFound();
        }

        public static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", string.Empty);
            return output;
        }

        public static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            // 62nd char of encoding
            output = output.Replace('-', '+');
            // 63rd char of encoding
            output = output.Replace('_', '/');
            // Pad with trailing '='s
            switch (output.Length % 4)
            {
                case 0:
                    // No pad chars in this case
                    break;
                case 2:
                    // Two pad chars
                    output += "=="; break;
                case 3:
                    // One pad char
                    output += "="; break;
                default:
                    throw new InvalidOperationException("Illegal base64url string!");
            }

            // Standard base64 decoder
            return Convert.FromBase64String(output);
        }


        //[Authorize(Roles = Role.Admin)]
        [Authorize]
        public IActionResult MarketKayit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MarketKayit(Market gelenMarket)
        {
            if (ModelState.IsValid)
            {


                var kullanici = await _userManager.GetUserAsync(User);
                var ayar = await _context.SiteAyari.FindAsync(1);


                CreateSubMerchantRequest request = new CreateSubMerchantRequest
                {
                    Locale = Locale.TR.ToString(),
                    ConversationId = kullanici.Id + "_Istek",
                    SubMerchantExternalId = kullanici.Id,
                    SubMerchantType = SubMerchantType.PERSONAL.ToString(),
                    Address = gelenMarket.Adress,
                    ContactName = gelenMarket.contactName,
                    ContactSurname = gelenMarket.contactSurname,
                    Email = kullanici.Email,
                    GsmNumber = kullanici.Telefon_Numarasi,
                    Name = kullanici.UserName,
                    Iban = gelenMarket.IBAN,
                    IdentityNumber = gelenMarket.TCKIMLIKNO,
                    Currency = Currency.TRY.ToString()
                };

                SubMerchant subMerchant = SubMerchant.Create(request, new Options() { ApiKey = ayar.IyzicoApiKey, BaseUrl = ayar.IyzicoBaseURL, SecretKey = ayar.IyzicoApiSecret });
                

                if (subMerchant.Status == Status.SUCCESS.ToString())
                {
                    gelenMarket.MarketID = kullanici.Id;
                    gelenMarket.SubMerchantKey = subMerchant.SubMerchantKey;
                    await _context.Marketler.AddAsync(gelenMarket);
                    kullanici.MarketHesabimi = true;
                    kullanici.MarketProfili = gelenMarket;
                    _context.Kullanicilar.Update(kullanici);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Basarili");
                }
                else
                {
                    string mesaj = "Iyzico bir hata döndürdü \"" + subMerchant.ErrorMessage + "\" Hata kodu: " +
                                   subMerchant.ErrorCode;
                    ModelState.AddModelError(string.Empty, mesaj);
                    return View(gelenMarket);
                }
            }
            else
            {
                return View(gelenMarket);
            }
        }

        public IActionResult HesapKilitli()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Basarili() => View();
        
        public async Task<IActionResult> Panel()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("GirisYap", "Hesap");
            }
            var gelenKullanici = await kullaniciGetir(User.Identity.Name);
           // var gelenSatis = await _context.Satislar.Where(x => x.Alan_Kullanici.UserName == gelenKullanici.UserName && x.odendiMi == true).ToListAsync();
            await EnSonOturumAcma(gelenKullanici);
            //ViewBag.OkunmamisMesajsayisi = _context.Mesajlar.AsNoTracking().Where(x => x.Alan_Kullanici.UserName == User.Identity.Name).Select(x => x.OkunduMu).Where(x => x == false).ToList().Count();
            return View();
        }

        public async Task<IActionResult> hesapbilgilerim() => View();
        
        public async Task<IActionResult> kullanicibilgi()
        {
            var kullanici = await kullaniciGetir(User.Identity.Name);
            KullaniciBilgileriViewModel KBvm = new KullaniciBilgileriViewModel();
            KBvm.Email = kullanici.Email;
            KBvm.Ad = kullanici.Ad;
            KBvm.Soyad = kullanici.Soyad;
            KBvm.TelefonNumarasi = kullanici.Telefon_Numarasi;
            return View(KBvm);
        }




        [AllowAnonymous]
        public async Task<IActionResult> SifremiUnuttum()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SifremiUnuttum(string email)
        {
            if (email!="")
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return RedirectToAction(nameof(SifremiUnuttum));

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action("sifresifirlama", "Hesap", new { token, email = user.Email }, Request.Scheme);


                EmailYolla(email, "Şifre sıfırlama bağlantınız " + callback, "Şifre Sıfırlama Bağlantısı");
                return RedirectToAction("SifremiUnuttumEposta");
            }
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> SifremiUnuttumEposta()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> sifresifirlama(string email, string token)
        {
            if (email!=""&&token!="")
            {
                SifreSifirlamaVM ssVm = new SifreSifirlamaVM();
                ssVm.email = email;
                ssVm.Token = token;
                return View(ssVm);
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> sifresifirlama(SifreSifirlamaVM ssVm)
        {
            if (!ModelState.IsValid)
                return View(ssVm);

            var user = await _userManager.FindByEmailAsync(ssVm.email);
            if (user == null)
                RedirectToAction(nameof(SifremiUnuttum));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, ssVm.Token, ssVm.sifre);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View();
            }

            return RedirectToAction("SifreSifirlamaBasarili");
        }

        public async Task<IActionResult> SifreSifirlamaBasarili()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Adresler()
        {
            var kullanici = await kullaniciGetir(User.Identity.Name);
            List<AdresGosterVM> agList = new List<AdresGosterVM>();
            foreach (Adres item in kullanici.Adresler)
            {
                AdresGosterVM agvm = new AdresGosterVM()
                {
                    AdresAd = item.AdresName,
                    ID = item.adresID,
                    Ilce = item.ilce,
                    desc = item.description
                };
                agList.Add(agvm);
            }
            return View(agList);
        }

        [Authorize]
        public async Task<IActionResult> Siparislerim()
        {
            var gelenSatis = await _context.Satislar.Include(x=>x.SatisDurumu).Include(x=>x.Satan_Market).Include(x=>x.SatilanUrun).Where(x => x.Alan_Kullanici.UserName == User.Identity.Name && x.odendiMi == true).OrderByDescending(x=>x.SatisID).ToListAsync();
            return View(gelenSatis);
        }

        [Authorize]
        [Route("hesap/siparisdetay/{id}")]
        public async Task<IActionResult> siparisdetay(int id)
        {
            var satisdetay = await _context.Satislar.Include(x=>x.verilenAdres).Include(x=>x.SatisDurumu).Include(x=>x.Satan_Market).Include(x => x.SatilanUrun).FirstOrDefaultAsync(x => x.Alan_Kullanici.UserName == User.Identity.Name && x.SatisID==id);
            return View(satisdetay);
        }
        
        
        
        [Authorize]
        public IActionResult AdresEkle()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdresEkle(AdresEkleViewModel aevm)
        {
            var kullanici = await kullaniciGetir(User.Identity.Name);
            
            
            if (ModelState.IsValid)
            {
                Adres a = new Adres()
                {
                    AdresName = aevm.AdresName,
                    ilce = aevm.ilce,
                    city = aevm.il,
                    country = "Azerbeycan",
                    description = aevm.description,
                    namesurname = aevm.namesurname,
                    zipcode = aevm.zipcode
                };

                await _context.Adresler.AddAsync(a);
                await _context.SaveChangesAsync();
                kullanici.Adresler.Add(a);
                _context.Kullanicilar.Update(kullanici);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdresSec");
            }
            return View(aevm);
        }
        [Authorize]
        public async Task<IActionResult> AdresSec()
        {
            var kullanici = await kullaniciGetir(User.Identity.Name);
            if (kullanici.Adresler.Count() > 0)
            {
                List<AdresGosterVM> agvmList = new List<AdresGosterVM>();
                foreach (Adres item in kullanici.Adresler)
                {
                    AdresGosterVM agvm = new AdresGosterVM()
                    {
                        AdresAd = item.AdresName,
                        ID = item.adresID,
                        Ilce = item.ilce,
                        desc = item.description
                    };
                    agvmList.Add(agvm);
                }
                
                return View(agvmList);
            }

            return RedirectToAction("AdresEkle");
        }
        
        
        [AllowAnonymous]
        public async Task<IActionResult> Kur()
        {
            await KategoriKur(_context);
            return View();
        }

        public async Task KategoriKur(ApplicationDbContext context)
        {
            if (context != null)
            {
                Kategori kadin = new Kategori()
                {
                    KategoriAd = "kadin",
                    KategoriAciklama = "Kadınlar için ürünler",
                    kisaad = "kadin",
                    AnaKategoriMi = true
                };
                Kategori erkek = new Kategori()
                {
                    KategoriAd = "Erkek",
                    KategoriAciklama = "Erkekler için ürünler",
                    kisaad = "erkek",
                    AnaKategoriMi = true
                };
                Kategori cocuk = new Kategori()
                {
                    KategoriAd = "Çocuk",
                    KategoriAciklama = "Çocuklar için ürünler",
                    kisaad = "cocuk",
                    AnaKategoriMi = true
                };
                Kategori evyasam = new Kategori()
                {
                    KategoriAd = "Ev & Yaşam",
                    KategoriAciklama = "Ev malzemelerinden, hobilere kadar",
                    kisaad = "evyasam",
                    AnaKategoriMi = true
                };
                Kategori supermarket = new Kategori()
                {
                    KategoriAd = "Süpermarket",
                    KategoriAciklama = "Süpermarket ihtiyaçlarınız burada",
                    kisaad = "supermarket",
                    AnaKategoriMi = true
                };
                Kategori kozmetik = new Kategori()
                {
                    KategoriAd = "Kozmetik",
                    KategoriAciklama = "Kozmetik ürünleri",
                    kisaad = "kozmetik",
                    AnaKategoriMi = true
                };
                Kategori ayakkabi = new Kategori()
                {
                    KategoriAd = "Ayakkabı & Çanta",
                    KategoriAciklama = "Ayakkabı ve çanta ürünlerimiz",
                    kisaad = "ayakkabicanta",
                    AnaKategoriMi = true
                };
                Kategori saat = new Kategori()
                {
                    KategoriAd = "Saat & Aksesuar",
                    KategoriAciklama = "Aksesuar ürünlerimiz",
                    kisaad = "saatveaksesuar",
                    AnaKategoriMi = true
                };
                Kategori elektirk = new Kategori()
                {
                    KategoriAd = "Elektronik",
                    KategoriAciklama = "Televizyondan yazıcıya elektronik olan ürünler",
                    kisaad = "elektronik",
                    AnaKategoriMi = true
                };
                await context.Kategoriler.AddAsync(kadin);
                await context.Kategoriler.AddAsync(erkek);
                await context.Kategoriler.AddAsync(cocuk);
                await context.Kategoriler.AddAsync(evyasam);
                await context.Kategoriler.AddAsync(supermarket);
                await context.Kategoriler.AddAsync(kozmetik);
                await context.Kategoriler.AddAsync(ayakkabi);
                await context.Kategoriler.AddAsync(saat);
                await context.Kategoriler.AddAsync(elektirk);
                await context.SaveChangesAsync();
            }

        }




        //public async Task KategoriKur(ApplicationDbContext context)
        //{
        //    if (context != null)
        //    {
        //        Kategori Grafik = new Kategori()
        //        {
        //            KategoriAd = "Grafik & Tasarım",
        //            KategoriAciklama = "Grafik ve tasarım ile ilgili herşey",
        //            kisaad = "grafiktasarim"
        //        };
        //        Kategori ses = new Kategori()
        //        {
        //            KategoriAd = "Müzik & Ses",
        //            KategoriAciklama = "Müzik ve ses ile ilgili herşey",
        //            kisaad = "sesmuzik"
        //        };
        //        Kategori Video = new Kategori()
        //        {
        //            KategoriAd = "Video & Animasyon",
        //            KategoriAciklama = "Video ve Animasyon ile ilgili herşey",
        //            kisaad = "videoanim"
        //        };
        //        Kategori yazi = new Kategori()
        //        {
        //            KategoriAd = "Yazı & Çeviri",
        //            KategoriAciklama = "Yazı ve Çeviri ile ilgili herşey",
        //            kisaad = "yaziceviri"
        //        };
        //        Kategori internet = new Kategori()
        //        {
        //            KategoriAd = "İnternet Reklamcılığı",
        //            KategoriAciklama = "İnternet reklamcılığı ile ilgili herşey",
        //            kisaad = "internetreklam"
        //        };
        //        Kategori Yasam = new Kategori()
        //        {
        //            KategoriAd = "Yaşam & Eğlence",
        //            KategoriAciklama = "Yaşam ve Eğlence ile ilgili herşey",
        //            kisaad = "yasameglence"
        //        };
        //        Kategori hediye = new Kategori()
        //        {
        //            KategoriAd = "Hediye",
        //            KategoriAciklama = "Hediye ile ilgili herşey",
        //            kisaad = "hediye"
        //        };
        //        Kategori baski = new Kategori()
        //        {
        //            KategoriAd = "Baskı",
        //            KategoriAciklama = "Basım işleri",
        //            kisaad = "baski"
        //        };
        //        Kategori Yazilim = new Kategori()
        //        {
        //            KategoriAd = "Yazılım & Teknoloji",
        //            KategoriAciklama = "Yazılım ve Teknoloji ile ilgili herşey",
        //            kisaad = "yazilim"
        //        };
        //        Kategori isveyonetim = new Kategori()
        //        {
        //            KategoriAd = "İş & Yönetim",
        //            KategoriAciklama = "İş ve Yönetim ile ilgili herşey",
        //            kisaad = "isveyonetim"
        //        };


        //        await context.Kategoriler.AddAsync(Grafik);
        //        await context.Kategoriler.AddAsync(ses);
        //        await context.Kategoriler.AddAsync(Video);
        //        await context.Kategoriler.AddAsync(yazi);
        //        await context.Kategoriler.AddAsync(internet);
        //        await context.Kategoriler.AddAsync(Yasam);
        //        await context.Kategoriler.AddAsync(hediye);
        //        await context.Kategoriler.AddAsync(baski);
        //        await context.Kategoriler.AddAsync(Yazilim);
        //        await context.Kategoriler.AddAsync(isveyonetim);

        //        await context.SaveChangesAsync();


        //        Kategori reklambanner = new Kategori()
        //        {
        //            KategoriAd = "Reklam & Banner Tasarımı",
        //            KategoriAciklama = "Reklam & Banner Tasarımı",
        //            kisaad = "reklambanner"
        //        };
        //        Kategori sosyalmedya = new Kategori()
        //        {
        //            KategoriAd = "Sosyal Medya Tasarımları",
        //            KategoriAciklama = "Sosyal Medya  Tasarımları",
        //            kisaad = "sosyalmedyatas"
        //        };
        //        Kategori ilus = new Kategori()
        //        {
        //            KategoriAd = "İllüstrasyon",
        //            KategoriAciklama = "İllüstrasyon",
        //            kisaad = "ilustrasyon"
        //        };
        //        Kategori karikatur = new Kategori()
        //        {
        //            KategoriAd = "Karikatür & Karakalem",
        //            KategoriAciklama = "Karikatür & Karakalem",
        //            kisaad = "karikaturkarakalem"
        //        };
        //        Kategori sunum = new Kategori()
        //        {
        //            KategoriAd = "Sunum & Infografik",
        //            KategoriAciklama = "Sunum & Infografik",
        //            kisaad = "sunuminfografik"
        //        };
        //        Kategori Portreler = new Kategori()
        //        {
        //            KategoriAd = "Portreler",
        //            KategoriAciklama = "Portreler",
        //            kisaad = "Portreler"
        //        };
        //        Kategori fotografduzenleme = new Kategori()
        //        {
        //            KategoriAd = "Fotoğraf & Düzenleme",
        //            KategoriAciklama = "Fotoğraf & Düzenleme",
        //            kisaad = "fotografduzenleme"
        //        };
        //        Kategori kurumsalkartvizit = new Kategori()
        //        {
        //            KategoriAd = "Kurumsal Kimlik & Kartvizit",
        //            KategoriAciklama = "Kurumsal Kimlik & Kartvizit",
        //            kisaad = "kurumsalkartvizit"
        //        };
        //        Kategori logo = new Kategori()
        //        {
        //            KategoriAd = "Logo Tasarımı",
        //            KategoriAciklama = "Logo Tasarımı",
        //            kisaad = "logotasarimi"
        //        };
        //        Kategori posterafis = new Kategori()
        //        {
        //            KategoriAd = "Poster & Afiş",
        //            KategoriAciklama = "Poster & Afiş",
        //            kisaad = "posterafis"
        //        };
        //        Kategori brosur = new Kategori()
        //        {
        //            KategoriAd = "Broşür & Katalog Tasarımı",
        //            KategoriAciklama = "Broşür & Katalog Tasarımı",
        //            kisaad = "brosurkatalog"
        //        };
        //        Kategori triditasarim = new Kategori()
        //        {
        //            KategoriAd = "3D Tasarımlar",
        //            KategoriAciklama = "3D Tasarımlar",
        //            kisaad = "triditasarim"
        //        };
        //        Kategori uiux = new Kategori()
        //        {
        //            KategoriAd = "UI/UX Tasarımı ( Arayüz )",
        //            KategoriAciklama = "UI/UX Tasarımı ( Arayüz )",
        //            kisaad = "uiux"
        //        };
        //        Kategori ambalaj = new Kategori()
        //        {
        //            KategoriAd = "Ambalaj Tasarımı",
        //            KategoriAciklama = "Ambalaj Tasarımı",
        //            kisaad = "ambalajtasarim"
        //        };
        //        Kategori tshirt = new Kategori()
        //        {
        //            KategoriAd = "T-shirt Tasarım",
        //            KategoriAciklama = "T-shirt Tasarım",
        //            kisaad = "tshirttasarim"
        //        };
        //        Kategori davetiye = new Kategori()
        //        {
        //            KategoriAd = "Davetiye Tasarımları",
        //            KategoriAciklama = "Davetiye Tasarımları",
        //            kisaad = "davetiyetasarim"
        //        };


        //        await context.Kategoriler.AddAsync(reklambanner);
        //        await context.Kategoriler.AddAsync(sosyalmedya);
        //        await context.Kategoriler.AddAsync(ilus);
        //        await context.Kategoriler.AddAsync(karikatur);
        //        await context.Kategoriler.AddAsync(sunum);
        //        await context.Kategoriler.AddAsync(Portreler);
        //        await context.Kategoriler.AddAsync(kurumsalkartvizit);
        //        await context.Kategoriler.AddAsync(fotografduzenleme);
        //        await context.Kategoriler.AddAsync(logo);
        //        await context.Kategoriler.AddAsync(posterafis);
        //        await context.Kategoriler.AddAsync(brosur);
        //        await context.Kategoriler.AddAsync(triditasarim);
        //        await context.Kategoriler.AddAsync(uiux);
        //        await context.Kategoriler.AddAsync(ambalaj);
        //        await context.Kategoriler.AddAsync(tshirt);
        //        await context.Kategoriler.AddAsync(davetiye);

        //        await context.SaveChangesAsync();

        //        Grafik.KategorininAlt.Add(reklambanner);
        //        Grafik.KategorininAlt.Add(sosyalmedya);
        //        Grafik.KategorininAlt.Add(ilus);
        //        Grafik.KategorininAlt.Add(karikatur);
        //        Grafik.KategorininAlt.Add(sunum);
        //        Grafik.KategorininAlt.Add(Portreler);
        //        Grafik.KategorininAlt.Add(kurumsalkartvizit);
        //        Grafik.KategorininAlt.Add(fotografduzenleme);
        //        Grafik.KategorininAlt.Add(logo);
        //        Grafik.KategorininAlt.Add(posterafis);
        //        Grafik.KategorininAlt.Add(brosur);
        //        Grafik.KategorininAlt.Add(triditasarim);
        //        Grafik.KategorininAlt.Add(uiux);
        //        Grafik.KategorininAlt.Add(ambalaj);
        //        Grafik.KategorininAlt.Add(tshirt);
        //        Grafik.KategorininAlt.Add(davetiye);

        //        context.Kategoriler.Update(Grafik);

        //        await context.SaveChangesAsync();

        //        Kategori seslendirme = new Kategori()
        //        {
        //            KategoriAd = "Seslendirme",
        //            KategoriAciklama = "Seslendirme",
        //            kisaad = "seslendirme"
        //        };
        //        Kategori mixmaster = new Kategori()
        //        {
        //            KategoriAd = "Mix & Mastering",
        //            KategoriAciklama = "Mix & Mastering",
        //            kisaad = "mixmastering"
        //        };
        //        Kategori sozyazarligi = new Kategori()
        //        {
        //            KategoriAd = "Söz Yazarlığı",
        //            KategoriAciklama = "Söz Yazarlığı",
        //            kisaad = "sozyazarligi"
        //        };
        //        Kategori jinglereklam = new Kategori()
        //        {
        //            KategoriAd = "Jingle & Reklam Müziği",
        //            KategoriAciklama = "Jingle & Reklam Müziği",
        //            kisaad = "jinglereklam"
        //        };

        //        await context.Kategoriler.AddAsync(seslendirme);
        //        await context.Kategoriler.AddAsync(mixmaster);
        //        await context.Kategoriler.AddAsync(sozyazarligi);
        //        await context.Kategoriler.AddAsync(jinglereklam);

        //        await context.SaveChangesAsync();

        //        ses.KategorininAlt.Add(seslendirme);
        //        ses.KategorininAlt.Add(mixmaster);
        //        ses.KategorininAlt.Add(sozyazarligi);
        //        ses.KategorininAlt.Add(jinglereklam);


        //        context.Kategoriler.Update(ses);

        //        await context.SaveChangesAsync();


        //        Kategori videoreklam = new Kategori()
        //        {
        //            KategoriAd = "Video Reklamları",
        //            KategoriAciklama = "Video Reklamları",
        //            kisaad = "videoreklam"
        //        };
        //        Kategori Animasyon = new Kategori()
        //        {
        //            KategoriAd = "Animasyon",
        //            KategoriAciklama = "Animasyon",
        //            kisaad = "animasyon"
        //        };
        //        Kategori prodiksiyon = new Kategori()
        //        {
        //            KategoriAd = "Video Prodüksiyon",
        //            KategoriAciklama = "Video Prodüksiyon",
        //            kisaad = "VideoProduksiyon"
        //        };
        //        Kategori oyunculuvideo = new Kategori()
        //        {
        //            KategoriAd = "Oyunculu tanıtım videoları",
        //            KategoriAciklama = "Oyunculu tanıtım videoları",
        //            kisaad = "oyunculuvideo"
        //        };
        //        Kategori slaytsunum = new Kategori()
        //        {
        //            KategoriAd = "Slayt & Sunum",
        //            KategoriAciklama = "Slayt & Sunum",
        //            kisaad = "slaytsunum"
        //        };
        //        Kategori cizgianimasyon = new Kategori()
        //        {
        //            KategoriAd = "Cizgi Animasyon",
        //            KategoriAciklama = "Cizgi Animasyon",
        //            kisaad = "cizgianimasyon"
        //        };

        //        await context.Kategoriler.AddAsync(videoreklam);
        //        await context.Kategoriler.AddAsync(Animasyon);
        //        await context.Kategoriler.AddAsync(prodiksiyon);
        //        await context.Kategoriler.AddAsync(oyunculuvideo);
        //        await context.Kategoriler.AddAsync(slaytsunum);
        //        await context.Kategoriler.AddAsync(cizgianimasyon);


        //        await context.SaveChangesAsync();

        //        Video.KategorininAlt.Add(videoreklam);
        //        Video.KategorininAlt.Add(Animasyon);
        //        Video.KategorininAlt.Add(prodiksiyon);
        //        Video.KategorininAlt.Add(oyunculuvideo);
        //        Video.KategorininAlt.Add(slaytsunum);
        //        Video.KategorininAlt.Add(cizgianimasyon);

        //        context.Kategoriler.Update(Video);

        //        await context.SaveChangesAsync();


        //        Kategori senaryo = new Kategori()
        //        {
        //            KategoriAd = "Senaryo",
        //            KategoriAciklama = "Senaryo",
        //            kisaad = "Senaryo"
        //        };
        //        Kategori ceviri = new Kategori()
        //        {
        //            KategoriAd = "Çeviri",
        //            KategoriAciklama = "Çeviri",
        //            kisaad = "ceviri"
        //        };
        //        Kategori webicerik = new Kategori()
        //        {
        //            KategoriAd = "Web & İçerik",
        //            KategoriAciklama = "Web & İçerik",
        //            kisaad = "webicerik"
        //        };
        //        Kategori metinyazalilk = new Kategori()
        //        {
        //            KategoriAd = "Metin Yazarlığı",
        //            KategoriAciklama = "Metin Yazarlığı",
        //            kisaad = "metinyazalilk"
        //        };
        //        Kategori seslendirmeyazi = new Kategori()
        //        {
        //            KategoriAd = "Seslendirme Yazıları",
        //            KategoriAciklama = "Seslendirme Yazıları",
        //            kisaad = "seslendirmeyazi"
        //        };
        //        Kategori reklamyazi = new Kategori()
        //        {
        //            KategoriAd = "Reklam Yazıları",
        //            KategoriAciklama = "Reklam Yazıları",
        //            kisaad = "reklamyazilari"
        //        };
        //        Kategori basinbulten = new Kategori()
        //        {
        //            KategoriAd = "Basın Bülteni",
        //            KategoriAciklama = "Basın Bülteni",
        //            kisaad = "basinbulteni"
        //        };
        //        Kategori siir = new Kategori()
        //        {
        //            KategoriAd = "Şiir",
        //            KategoriAciklama = "Şiir",
        //            kisaad = "siir"
        //        };
        //        Kategori cv = new Kategori()
        //        {
        //            KategoriAd = "CV Hazırlama",
        //            KategoriAciklama = "CV Hazırlama",
        //            kisaad = "cvhazirlama"
        //        };

        //        await context.Kategoriler.AddAsync(senaryo);
        //        await context.Kategoriler.AddAsync(ceviri);
        //        await context.Kategoriler.AddAsync(webicerik);
        //        await context.Kategoriler.AddAsync(metinyazalilk);
        //        await context.Kategoriler.AddAsync(seslendirmeyazi);
        //        await context.Kategoriler.AddAsync(reklamyazi);
        //        await context.Kategoriler.AddAsync(basinbulten);
        //        await context.Kategoriler.AddAsync(siir);
        //        await context.Kategoriler.AddAsync(cv);

        //        await context.SaveChangesAsync();

        //        yazi.KategorininAlt.Add(senaryo);
        //        yazi.KategorininAlt.Add(ceviri);
        //        yazi.KategorininAlt.Add(webicerik);
        //        yazi.KategorininAlt.Add(metinyazalilk);
        //        yazi.KategorininAlt.Add(seslendirmeyazi);
        //        yazi.KategorininAlt.Add(reklamyazi);
        //        yazi.KategorininAlt.Add(basinbulten);
        //        yazi.KategorininAlt.Add(siir);
        //        yazi.KategorininAlt.Add(cv);


        //        context.Kategoriler.Update(yazi);

        //        await context.SaveChangesAsync();


        //        Kategori astroloji = new Kategori()
        //        {
        //            KategoriAd = "Astroloji & Fal",
        //            KategoriAciklama = "Astroloji & Fal",
        //            kisaad = "astrolojifal"
        //        };
        //        Kategori smm = new Kategori()
        //        {
        //            KategoriAd = "SMM Hizmeti",
        //            KategoriAciklama = "SMM Hizmeti",
        //            kisaad = "smm"
        //        };


        //        await context.Kategoriler.AddAsync(astroloji);
        //        await context.Kategoriler.AddAsync(smm);
        //        await context.SaveChangesAsync();

        //        Yasam.KategorininAlt.Add(astroloji);
        //        Yasam.KategorininAlt.Add(smm);
        //        context.Kategoriler.Update(Yasam);

        //        await context.SaveChangesAsync();

        //        Kategori ismeozel = new Kategori()
        //        {
        //            KategoriAd = "İsme Özel Şarkı",
        //            KategoriAciklama = "İsme Özel Şarkı",
        //            kisaad = "ismeozel"
        //        };
        //        Kategori portrecizim = new Kategori()
        //        {
        //            KategoriAd = "Portre Çizim",
        //            KategoriAciklama = "Portre Çizim",
        //            kisaad = "portrecizim"
        //        };
        //        Kategori ozelklip = new Kategori()
        //        {
        //            KategoriAd = "Özel Klip",
        //            KategoriAciklama = "Özel Klip",
        //            kisaad = "ozelklip"
        //        };


        //        await context.Kategoriler.AddAsync(ismeozel);
        //        await context.Kategoriler.AddAsync(portrecizim);
        //        await context.Kategoriler.AddAsync(ozelklip);
        //        await context.SaveChangesAsync();

        //        hediye.KategorininAlt.Add(ismeozel);
        //        hediye.KategorininAlt.Add(portrecizim);
        //        hediye.KategorininAlt.Add(ozelklip);
        //        context.Kategoriler.Update(hediye);

        //        await context.SaveChangesAsync();

        //        Kategori kartvizit = new Kategori()
        //        {
        //            KategoriAd = "Kartvizit",
        //            KategoriAciklama = "Kartvizit Basımı",
        //            kisaad = "kartvizit"
        //        };
        //        Kategori elilanibrosur = new Kategori()
        //        {
        //            KategoriAd = "El İlanı & Broşür",
        //            KategoriAciklama = "El İlanı & Broşür",
        //            kisaad = "elilanibrosur"
        //        };
        //        Kategori reklamurunleri = new Kategori()
        //        {
        //            KategoriAd = "Reklam Ürünleri",
        //            KategoriAciklama = "Reklam Ürünleri",
        //            kisaad = "reklamurunleri"
        //        };
        //        Kategori caferestoran = new Kategori()
        //        {
        //            KategoriAd = "Cafe ve Restoran",
        //            KategoriAciklama = "Cafe ve Restoran",
        //            kisaad = "caferestoran"
        //        };

        //        await context.Kategoriler.AddAsync(kartvizit);
        //        await context.Kategoriler.AddAsync(elilanibrosur);
        //        await context.Kategoriler.AddAsync(reklamurunleri);
        //        await context.Kategoriler.AddAsync(caferestoran);
        //        await context.SaveChangesAsync();

        //        baski.KategorininAlt.Add(kartvizit);
        //        baski.KategorininAlt.Add(elilanibrosur);
        //        baski.KategorininAlt.Add(reklamurunleri);
        //        baski.KategorininAlt.Add(caferestoran);
        //        context.Kategoriler.Update(baski);

        //        await context.SaveChangesAsync();

        //        Kategori webyazilim = new Kategori()
        //        {
        //            KategoriAd = "Web Yazılım",
        //            KategoriAciklama = "Web Yazılım",
        //            kisaad = "webyazilim"
        //        };
        //        Kategori mobiluygulamagelistirme = new Kategori()
        //        {
        //            KategoriAd = "Mobil Uygulama Geliştirme",
        //            KategoriAciklama = "Mobil Uygulama Geliştirmer",
        //            kisaad = "mobiluygulamagelistirme"
        //        };
        //        Kategori eticaret = new Kategori()
        //        {
        //            KategoriAd = "E-Ticaret",
        //            KategoriAciklama = "E-Ticaret",
        //            kisaad = "eticaret"
        //        };
        //        Kategori hostingdomain = new Kategori()
        //        {
        //            KategoriAd = "Hosting & Domain",
        //            KategoriAciklama = "Hosting & Domain",
        //            kisaad = "hostingdomain"
        //        };
        //        Kategori oyun = new Kategori()
        //        {
        //            KategoriAd = "Oyun Geliştirme",
        //            KategoriAciklama = "Oyun Geliştirme",
        //            kisaad = "oyun"
        //        };
        //        Kategori Wordpress = new Kategori()
        //        {
        //            KategoriAd = "Wordpress",
        //            KategoriAciklama = "Wordpress",
        //            kisaad = "Wordpress"
        //        };

        //        await context.Kategoriler.AddAsync(Wordpress);
        //        await context.Kategoriler.AddAsync(webyazilim);
        //        await context.Kategoriler.AddAsync(mobiluygulamagelistirme);
        //        await context.Kategoriler.AddAsync(eticaret);
        //        await context.Kategoriler.AddAsync(hostingdomain);
        //        await context.Kategoriler.AddAsync(oyun);
        //        await context.SaveChangesAsync();

        //        Yazilim.KategorininAlt.Add(Wordpress);
        //        Yazilim.KategorininAlt.Add(webyazilim);
        //        Yazilim.KategorininAlt.Add(mobiluygulamagelistirme);
        //        Yazilim.KategorininAlt.Add(eticaret);
        //        Yazilim.KategorininAlt.Add(hostingdomain);
        //        Yazilim.KategorininAlt.Add(oyun);
        //        context.Kategoriler.Update(Yazilim);

        //        await context.SaveChangesAsync();


        //        Kategori sanalasistan = new Kategori()
        //        {
        //            KategoriAd = "Sanal Asistanlık",
        //            KategoriAciklama = "Sanal Asistanlık",
        //            kisaad = "sanalasistan"
        //        };
        //        Kategori verigirisi = new Kategori()
        //        {
        //            KategoriAd = "Veri Girişi",
        //            KategoriAciklama = "Veri Girişi",
        //            kisaad = "verigirisi"
        //        };
        //        Kategori musteritemsil = new Kategori()
        //        {
        //            KategoriAd = "Müşteri Temsilcisi",
        //            KategoriAciklama = "Müşteri Temsilcisi",
        //            kisaad = "musteritemsilci"
        //        };
        //        Kategori Raporlama = new Kategori()
        //        {
        //            KategoriAd = "Raporlama",
        //            KategoriAciklama = "Raporlama",
        //            kisaad = "raporlama"
        //        };
        //        Kategori danismanlik = new Kategori()
        //        {
        //            KategoriAd = "Danışmanlık",
        //            KategoriAciklama = "Danışmanlık",
        //            kisaad = "danismanlik"
        //        };

        //        await context.Kategoriler.AddAsync(sanalasistan);
        //        await context.Kategoriler.AddAsync(verigirisi);
        //        await context.Kategoriler.AddAsync(musteritemsil);
        //        await context.Kategoriler.AddAsync(Raporlama);
        //        await context.Kategoriler.AddAsync(danismanlik);

        //        await context.SaveChangesAsync();

        //        isveyonetim.KategorininAlt.Add(sanalasistan);
        //        isveyonetim.KategorininAlt.Add(verigirisi);
        //        isveyonetim.KategorininAlt.Add(musteritemsil);
        //        isveyonetim.KategorininAlt.Add(Raporlama);
        //        isveyonetim.KategorininAlt.Add(danismanlik);

        //        context.Kategoriler.Update(isveyonetim);

        //        await context.SaveChangesAsync();


        //        Kategori sosyalmedyapazar = new Kategori()
        //        {
        //            KategoriAd = "Sosyal medya pazarlamacılığı",
        //            KategoriAciklama = "Sosyal medya pazarlamacılığı",
        //            kisaad = "smpazarlama"
        //        };
        //        Kategori epostapazarlama = new Kategori()
        //        {
        //            KategoriAd = "E-posta pazarlama",
        //            KategoriAciklama = "E-posta pazarlama",
        //            kisaad = "epostapazarlama"
        //        };
        //        Kategori Anketler = new Kategori()
        //        {
        //            KategoriAd = "Anketler",
        //            KategoriAciklama = "Anketler",
        //            kisaad = "anket"
        //        };
        //        Kategori reklamanaliz = new Kategori()
        //        {
        //            KategoriAd = "Reklam analizi",
        //            KategoriAciklama = "Reklam analizi",
        //            kisaad = "reklamanaliz"
        //        };
        //        Kategori mobilpazarlamareklam = new Kategori()
        //        {
        //            KategoriAd = "Mobil Pazarlama ve Reklamcılık",
        //            KategoriAciklama = "Mobil Pazarlama ve Reklamcılık",
        //            kisaad = "mobilpazarlamareklam"
        //        };
        //        Kategori muziktanitim = new Kategori()
        //        {
        //            KategoriAd = "Müzik Tanıtımıı",
        //            KategoriAciklama = "Müzik Tanıtımı",
        //            kisaad = "muziktanitim"
        //        };
        //        Kategori webtrafik = new Kategori()
        //        {
        //            KategoriAd = "Web Trafiği",
        //            KategoriAciklama = "Web Trafiği",
        //            kisaad = "webtrafik"
        //        };
        //        Kategori reklamdanisma = new Kategori()
        //        {
        //            KategoriAd = "Reklam Danışmanlığı",
        //            KategoriAciklama = "Reklam Danışmanlığı",
        //            kisaad = "reklamdanisma"
        //        };
        //        Kategori reklamyonetim = new Kategori()
        //        {
        //            KategoriAd = "Reklam Yönetimi",
        //            KategoriAciklama = "Reklam Yönetimi",
        //            kisaad = "reklamyonetim"
        //        };
        //        Kategori seo = new Kategori()
        //        {
        //            KategoriAd = "Arama Motoru Optimizasyonu (SEO)",
        //            KategoriAciklama = "Arama Motoru Optimizasyonu (SEO)",
        //            kisaad = "seo"
        //        };

        //        await context.Kategoriler.AddAsync(sosyalmedyapazar);
        //        await context.Kategoriler.AddAsync(epostapazarlama);
        //        await context.Kategoriler.AddAsync(Anketler);
        //        await context.Kategoriler.AddAsync(reklamanaliz);
        //        await context.Kategoriler.AddAsync(mobilpazarlamareklam);
        //        await context.Kategoriler.AddAsync(muziktanitim);
        //        await context.Kategoriler.AddAsync(webtrafik);
        //        await context.Kategoriler.AddAsync(reklamdanisma);
        //        await context.Kategoriler.AddAsync(reklamyonetim);
        //        await context.Kategoriler.AddAsync(seo);


        //        await context.SaveChangesAsync();

        //        internet.KategorininAlt.Add(sosyalmedyapazar);
        //        internet.KategorininAlt.Add(epostapazarlama);
        //        internet.KategorininAlt.Add(Anketler);
        //        internet.KategorininAlt.Add(reklamanaliz);
        //        internet.KategorininAlt.Add(mobilpazarlamareklam);
        //        internet.KategorininAlt.Add(muziktanitim);
        //        internet.KategorininAlt.Add(webtrafik);
        //        internet.KategorininAlt.Add(reklamdanisma);
        //        internet.KategorininAlt.Add(reklamyonetim);
        //        internet.KategorininAlt.Add(seo);

        //        context.Kategoriler.Update(internet);

        //        await context.SaveChangesAsync();


        //    }


        //}
        public async Task<Kullanici> kullaniciGetir(string kullaniciAdi) => await _context.Kullanicilar.Where(x => x.UserName == kullaniciAdi).Include(x => x.Mesajlar).Include(x=>x.Adresler).FirstOrDefaultAsync();
    }
}
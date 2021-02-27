using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TurkAzn.Web.Data;
using TurkAzn.Web.Models;

namespace TurkAzn.Web.Controllers
{
    public class AnasayfaController : Controller
    {
        private readonly ILogger<AnasayfaController> _logger;
        private readonly ApplicationDbContext _context;
      
        public AnasayfaController(ILogger<AnasayfaController> logger,ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            ViewBag.Kategori = _context.Kategoriler.Where(x=>x.AnaKategoriMi==true).ToList();
            return View();
        }

        
        public IActionResult Kategori()
        {
            var kategoriler = _context.Kategoriler.Include(x=>x.KategorininAlt).Where(x=>x.AnaKategoriMi==true).ToList();
            return View(kategoriler);
        }

        
        public IActionResult Favori()
        {
            return View();
        }

        
        public IActionResult Sepetim()
        {
            return View();
        }

        public IActionResult h404()
        {
            return View();
        }
        
        public IActionResult Hesabim()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

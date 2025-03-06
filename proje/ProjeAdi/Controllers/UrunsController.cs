using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjeAdi.Data;
using ProjeAdi.Models;

namespace ProjeAdi.Controllers
{
    public class UrunsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UrunsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Telefonlar & Tabletler
        public async Task<IActionResult> Telefonlar()
        {
            var urunler = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 2) // Telefon kategorisi
                .ToListAsync();
            return View("Index", urunler);
        }

        // Bilgisayarlar
        public async Task<IActionResult> Bilgisayarlar()
        {
            var urunler = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 1) // Bilgisayar kategorisi
                .ToListAsync();
            return View("Index", urunler);
        }

        // Akıllı Saatler & Aksesuarlar
        public async Task<IActionResult> Aksesuarlar()
        {
            var urunler = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 4) // Aksesuar kategorisi
                .ToListAsync();
            return View("Index", urunler);
        }

        // Kulaklıklar
        public async Task<IActionResult> Kulakliklar()
        {
            var urunler = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 3) // Kulaklık kategorisi
                .ToListAsync();
            return View("Index", urunler);
        }

        // Ev Aletleri
        public async Task<IActionResult> EvAletleri()
        {
            var urunler = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == 5) // Ev Aletleri kategorisi
                .ToListAsync();
            return View("Index", urunler);
        }

        // Ürün detay sayfası
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }
    }
} 
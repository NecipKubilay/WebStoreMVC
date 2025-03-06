using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjeAdi.Data;
using ProjeAdi.Models;

namespace ProjeAdi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin paneli ana sayfası - Ürün listesi
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // Yeni ürün ekleme sayfası
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Ad");
            return View();
        }

        // Yeni ürün ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,ImageUrl,CategoryId")] Product product)
        {
            Console.WriteLine("Create methodu çağrıldı");
            Console.WriteLine($"Ürün Adı: {product.Name}");
            Console.WriteLine($"Kategori ID: {product.CategoryId}");
            Console.WriteLine($"Fiyat: {product.Price}");
            Console.WriteLine($"Açıklama: {product.Description}");
            Console.WriteLine($"Resim URL: {product.ImageUrl}");
            
            // Kategori kontrolü
            var kategori = await _context.Categories.FindAsync(product.CategoryId);
            if (kategori == null)
            {
                Console.WriteLine("HATA: Seçilen kategori bulunamadı!");
                ModelState.AddModelError("CategoryId", "Seçilen kategori bulunamadı.");
            }

            // Fiyat kontrolü
            if (product.Price <= 0)
            {
                Console.WriteLine("HATA: Fiyat 0'dan büyük olmalıdır!");
                ModelState.AddModelError("Price", "Fiyat 0'dan büyük olmalıdır.");
            }

            // İsim kontrolü
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                Console.WriteLine("HATA: Ürün adı boş olamaz!");
                ModelState.AddModelError("Name", "Ürün adı boş olamaz.");
            }

            if (ModelState.IsValid)
            {
                try 
                {
                    Console.WriteLine("ModelState geçerli, veritabanına ekleme başlıyor");
                    _context.Add(product);
                    Console.WriteLine("Context'e ürün eklendi");
                    var result = await _context.SaveChangesAsync();
                    Console.WriteLine($"SaveChangesAsync sonucu: {result}");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Veritabanı Hatası: {ex.Message}");
                    Console.WriteLine($"Hata Detayı: {ex.StackTrace}");
                    ModelState.AddModelError("", "Veritabanına kayıt yapılırken bir hata oluştu.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Genel Hata: {ex.Message}");
                    Console.WriteLine($"Hata Detayı: {ex.StackTrace}");
                    ModelState.AddModelError("", "Beklenmeyen bir hata oluştu.");
                }
            }
            else
            {
                Console.WriteLine("ModelState geçersiz:");
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Hata: {modelError.ErrorMessage}");
                }
            }

            // Hata durumunda kategorileri tekrar yükle
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Ad");
            return View(product);
        }

        // Ürün düzenleme sayfası
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Ad");
            return View(product);
        }

        // Ürün düzenleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ImageUrl,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Ad");
            return View(product);
        }

        // Ürün silme sayfası
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Ürün silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(OrderDetails), new { id });
        }
    }
} 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjeAdi.Data;
using ProjeAdi.Models;
using System.Text.Json;

namespace ProjeAdi.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            return View(new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(i => i.Price * i.Quantity),
                Status = "Beklemede"
            });
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", order);
            }

            var cart = GetCart();
            if (!cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            order.OrderDate = DateTime.Now;
            order.Status = "Beklemede";
            order.TotalAmount = cart.Sum(i => i.Price * i.Quantity);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Subtotal = item.Price * item.Quantity,
                    ImageUrl = item.ImageUrl,
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName
                };

                _context.OrderItems.Add(orderItem);
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("OrderConfirmation", new { id = order.Id });
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }
    }
} 
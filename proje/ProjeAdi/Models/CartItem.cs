using System.ComponentModel.DataAnnotations;

namespace ProjeAdi.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public decimal Subtotal => Price * Quantity;
    }
} 
using System.ComponentModel.DataAnnotations;

namespace ProjeAdi.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        
        [Required]
        public string ProductName { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal Subtotal { get; set; }
        
        public string ImageUrl { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public string CategoryName { get; set; }

        // Navigation property
        [Required]
        public Order Order { get; set; }
    }
} 
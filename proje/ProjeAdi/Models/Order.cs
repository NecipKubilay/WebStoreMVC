using System.ComponentModel.DataAnnotations;

namespace ProjeAdi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Beklemede"; // Varsayılan değer atandı
        
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur")]
        [Display(Name = "Ad Soyad")]
        public string CustomerName { get; set; }
        
        [Required(ErrorMessage = "E-posta alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public string CustomerEmail { get; set; }
        
        [Required(ErrorMessage = "Telefon alanı zorunludur")]
        [Display(Name = "Telefon")]
        public string CustomerPhone { get; set; }
        
        [Required(ErrorMessage = "Teslimat adresi zorunludur")]
        [Display(Name = "Teslimat Adresi")]
        public string ShippingAddress { get; set; }
        
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
} 
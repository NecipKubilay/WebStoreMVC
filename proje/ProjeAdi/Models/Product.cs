using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjeAdi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ürün açıklaması zorunludur")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ürün fiyatı zorunludur")]
        [Display(Name = "Fiyat")]
        [Range(0, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        public decimal Price { get; set; }

        [Display(Name = "Görsel URL")]
        public string? ImageUrl { get; set; }

        // Kategori ilişkisi
        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Display(Name = "Kategori")]
        public Category Category { get; set; }

        public Product()
        {
            Name = string.Empty;
            Description = string.Empty;
            Category = new Category();
        }
    }
} 
using System.ComponentModel.DataAnnotations;

namespace ProjeAdi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [Display(Name = "Kategori Adı")]
        public string Ad { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public Category()
        {
            Ad = string.Empty;
            Description = string.Empty;
        }
    }
} 
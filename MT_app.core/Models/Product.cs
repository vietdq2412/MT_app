using System.ComponentModel.DataAnnotations;

namespace MT_app.core.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string? Image { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public long? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
namespace MT_app.core.Models
{
    public class Supplier : BaseEntity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
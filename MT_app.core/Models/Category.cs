namespace MT_app.core.Models
{
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public ICollection<Product>? Products { get; set; } = null;
    }
}
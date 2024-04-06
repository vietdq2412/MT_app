namespace MT_app.core.Models
{
    public class Order : BaseEntity
    {
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public decimal? TotalPrice { get; set; }
        public AppUser? AppUser { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
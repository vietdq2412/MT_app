namespace MT_app.core.Models
{
    public class OrderDetail : BaseEntity
    {
        public decimal? TotalPrice { get; set; }
        public long Quantity { get; set; }
        public long? ProductId { get; set; }
        public Product? Product { get; set; }
        public long? OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
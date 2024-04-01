namespace MT_app.core.Models
{
    public class Order : BaseEntity
    {
        public string? Name { get; set; }
        public string? Status { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
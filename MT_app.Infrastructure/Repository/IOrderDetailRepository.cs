using Microsoft.EntityFrameworkCore;
using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface IOrderDetailRepository : IBaseRepository<OrderDetail>
    {
        Task<List<OrderDetail>> FindOrderItemsInCartByUsername(string username);
        Task<List<OrderDetail>> FindOrderingItemsByProductIdAndOrderId(long? productId, long? orderId);
    }

    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext DbContext { get; }

        public OrderDetailRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.DbContext = appDbContext;
        }

        public async Task<List<OrderDetail>> FindOrderItemsInCartByUsername(string username)
        {
            return await DbContext.OrderDetails
                .Where(od => od.Order.AppUser.Email == username)
                .Where(od => od.Order.Status == OrderStatus.Ordering.ToString())
                .Include(od => od.Product)
                .ToListAsync();
        }

        public async Task<List<OrderDetail>> FindOrderingItemsByProductIdAndOrderId(long? id, long? orderId)
        {
            return await DbContext.OrderDetails
                .Where(od => od.ProductId == id)
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                .ToListAsync();
        }
    }
}
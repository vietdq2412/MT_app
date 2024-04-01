using Microsoft.EntityFrameworkCore;
using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface IOrderDetailRepository : IBaseRepository<OrderDetail>
    {
        Task<List<OrderDetail>> findNotOrderedItem();
        Task<List<OrderDetail>> findItemsNotOrderedByProductId(long? id);
    }

    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext DbContext { get; }

        public OrderDetailRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.DbContext = appDbContext;
        }

        public async Task<List<OrderDetail>> findNotOrderedItem()
        {
            return await DbContext.OrderDetails
                .Where(od => od.OrderId == null)
                .Where(od => od.Order == null)
                .Include(od => od.Product)
                .ToListAsync();
        }

        public async Task<List<OrderDetail>> findItemsNotOrderedByProductId(long? id)
        {
            return await DbContext.OrderDetails
                .Where(od => od.ProductId == id)
                .Where(od => od.Order == null)
                .Include(od => od.Product)
                .ToListAsync();
        }
    }
}
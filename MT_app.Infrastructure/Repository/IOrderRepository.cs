using Microsoft.EntityFrameworkCore;
using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> FindByStatus(string status);
    }

    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private ApplicationDbContext DbContext { get; }

        public OrderRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.DbContext = appDbContext;
        }

        public async Task<List<Order>> FindByStatus(string status)
        {
            return await DbContext.Orders
                .Where(o => o.Status == status)
                .ToListAsync();
        }
    }
}
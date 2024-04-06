using Microsoft.EntityFrameworkCore;
using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> FindByStatus(string status);
        Task<List<Order>> FindOrdersByUsernameAndStatus(string username, string status);
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

        public async Task<List<Order>> FindOrdersByUsernameAndStatus(string username, string status)
        {
            return await DbContext.Orders
                .Where(o => o.Status == status)
                .Where(o => o.AppUser!.Email == username)
                .ToListAsync();
        }
    }
}
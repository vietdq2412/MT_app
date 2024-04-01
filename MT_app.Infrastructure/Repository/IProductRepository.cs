using Microsoft.EntityFrameworkCore;
using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
    }

    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private ApplicationDbContext DbContext { get; }

        public ProductRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.DbContext = appDbContext;
        }

        public new async Task<List<Product?>> FindAll()
        {
            return (await DbContext.Products
                .Include(p => p.Supplier)
                .ToListAsync())!;
        }

        public Task<Product?> FindByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
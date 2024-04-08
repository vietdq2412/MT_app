using Microsoft.EntityFrameworkCore;
using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> SearchByCategoryAndProductName(long categoryId, string productName);
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

        public async Task<List<Product>> SearchByCategoryAndProductName(long categoryId, string productName)
        {
            IQueryable<Product> query = DbContext.Products;

            if (categoryId != -1)
            {
                query = query.Where(p => p.Categories!.Any(c => c.Id == categoryId)) ;
            }

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.Where(p => p.Name.Contains(productName));
            }

            return await query.ToListAsync();
        }

        public Task<Product?> FindByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
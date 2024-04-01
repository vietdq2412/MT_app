using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface ISupplierRepository : IBaseRepository<Supplier>
    {
    }

    public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        private ApplicationDbContext DbContext { get; }

        public SupplierRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.DbContext = appDbContext;
        }

        public Task<Supplier> FindByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
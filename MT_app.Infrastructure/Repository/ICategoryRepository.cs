using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
    }

    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private ApplicationDbContext DbContext { get; }

        public CategoryRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.DbContext = appDbContext;
        }

        public Task<Category> FindByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
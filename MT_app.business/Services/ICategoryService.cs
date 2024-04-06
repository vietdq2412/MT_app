using MT_app.core.Models;
using MT_app.Infrastructure.Repository;

namespace MT_app.business.Services
{
    public interface ICategoryService : IBaseService<Category>
    {
    }

    public class CategoryService : ICategoryService
    {
        public ICategoryRepository categoryRepository { get; }

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task Save(Category category)
        {
            await categoryRepository.Add(category);
        }

        public async Task<List<Category>> FindAll()
        {
            List<Category> list = (await categoryRepository.FindAll())!;
            return list;
        }

        public Task<Category?> FindById(long? id)
        {
            return categoryRepository.FindById(id);
        }


        public Task Delete(long id)
        {
            return categoryRepository.Delete(id);
        }

        public Task Update(Category t)
        {
            throw new NotImplementedException();
        }
    }
}
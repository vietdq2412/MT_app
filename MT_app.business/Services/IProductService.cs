using MT_app.core.Models;
using MT_app.Infrastructure.Repository;

namespace MT_app.business.Services
{
    public interface IProductService : IBaseService<Product>
    {
    }

    public class ProductService : IProductService
    {
        public IProductRepository productRepository { get; }

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task Save(Product product)
        {
            await productRepository.Add(product);
        }

        public async Task<List<Product>> FindAll()
        {
            List<Product> list = (await productRepository.FindAll())!;
            return list;
        }

        public async Task<Product?> FindById(long? id)
        {
            return await productRepository.FindById(id);
        }

        public Task Delete(long id)
        {
            return productRepository.Delete(id);
        }


        public Task Update(long id)
        {
            return productRepository.Update(productRepository.FindById(id).Result!);
        }
    }
}
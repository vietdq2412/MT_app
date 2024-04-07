using MT_app.core.Models;
using MT_app.Infrastructure.Repository;

namespace MT_app.business.Services
{
    public interface ISupplierService : IBaseService<Supplier>
    {
    }

    public class SupplierService : ISupplierService
    {
        public ISupplierRepository supplierRepository { get; }

        public SupplierService(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public async Task Save(Supplier supplier)
        {
            await supplierRepository.Add(supplier);
        }

        public async Task<List<Supplier>> FindAll()
        {
            List<Supplier> list = (await supplierRepository.FindAll())!;
            return list;
        }

        public Task<Supplier?> FindById(long? id)
        {
            return supplierRepository.FindById(id);
        }

        public Task Delete(Supplier supplier)
        {
            return supplierRepository.Delete(supplier);
        }

        public async Task Update(Supplier t)
        {
            await supplierRepository.Update(t);
        }
    }
}
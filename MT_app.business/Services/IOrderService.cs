using MT_app.core.Models;
using MT_app.Infrastructure.Repository;

namespace MT_app.business.Services
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<List<Order>> FindByStatus(string status);
        Task<List<Order>> FindOrdersByUsernameAndStatus(string username, string status);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository oderRepository;

        public OrderService(IOrderRepository oderRepository)
        {
            this.oderRepository = oderRepository;
        }

        public async Task Save(Order order)
        {
            await oderRepository.Add(order);
        }

        public Task<List<Order>> FindAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> FindById(long? id)
        {
            return await oderRepository.FindById(id);
        }

        public async Task<List<Order>> FindByStatus(string status)
        {
            return await oderRepository.FindByStatus(status);
        }

        public async Task<List<Order>> FindOrdersByUsernameAndStatus(string username, string status)
        {
            return await oderRepository.FindOrdersByUsernameAndStatus(username, status);
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task Update(long id)
        {
            throw new NotImplementedException();
        }
    }
}
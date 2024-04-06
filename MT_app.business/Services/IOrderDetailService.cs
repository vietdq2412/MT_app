using MT_app.core.Models;
using MT_app.Infrastructure.Repository;

namespace MT_app.business.Services
{
    public interface IOrderDetailService : IBaseService<OrderDetail>
    {
        Task<List<OrderDetail>> FindOrderingItems(string username);
    }

    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository oderDetailRepository;
        private readonly IOrderRepository orderRepository;

        public OrderDetailService(IOrderDetailRepository oderDetailRepository,
            IOrderRepository orderRepository)
        {
            this.oderDetailRepository = oderDetailRepository;
            this.orderRepository = orderRepository;
        }

        public async Task Save(OrderDetail orderDetail)
        {
            List<OrderDetail> od = await FindOrderingItemsByProductId(orderDetail.ProductId, orderDetail.Order.Id);
            if (od.Any())
            {
                OrderDetail existOd = od[0];
                existOd.Quantity += orderDetail.Quantity;
                await oderDetailRepository.Update(existOd);
            }
            else
            {
                await oderDetailRepository.Add(orderDetail);
            }
        }

        public async Task<List<OrderDetail>> FindAll()
        {
            return (await oderDetailRepository.FindAll())!;
        }

        public Task<OrderDetail?> FindById(long? id)
        {
            return oderDetailRepository.FindById(id);
        }

        public async Task<List<OrderDetail>> FindOrderingItems(string username)
        {
            return await oderDetailRepository.FindOrderItemsInCartByUsername(username);
        }

        public async Task<List<OrderDetail>> FindOrderingItemsByProductId(long? productId, long? orderId)
        {
            return await oderDetailRepository.FindOrderingItemsByProductIdAndOrderId(productId, orderId);
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
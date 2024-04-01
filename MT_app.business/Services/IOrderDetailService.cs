using MT_app.core.Models;
using MT_app.Infrastructure.Repository;

namespace MT_app.business.Services
{
    public interface IOrderDetailService : IBaseService<OrderDetail>
    {
        List<OrderDetail> FindItemsNotOrdered();
        List<OrderDetail> FindItemNotOrderedByProductId(long? id);
    }

    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository oderDetailRepository;

        public OrderDetailService(IOrderDetailRepository oderRepository)
        {
            this.oderDetailRepository = oderRepository;
        }

        public Task Save(OrderDetail orderDetail)
        {
            List<OrderDetail> od = FindItemNotOrderedByProductId(orderDetail.ProductId);
            if (od.Any())
            {
                OrderDetail existOd = od[0];
                existOd.Quantity += orderDetail.Quantity;
                orderDetail = existOd;
                return oderDetailRepository.Update(orderDetail);
            }

            return oderDetailRepository.Add(orderDetail);
        }

        public async Task<List<OrderDetail>> FindAll()
        {
            return (await oderDetailRepository.FindAll())!;
        }

        public Task<OrderDetail?> FindById(long? id)
        {
            return oderDetailRepository.FindById(id);
        }

        public List<OrderDetail> FindItemsNotOrdered()
        {
            return oderDetailRepository.findNotOrderedItem()
                .Result;
        }

        public List<OrderDetail> FindItemNotOrderedByProductId(long? id)
        {
            return oderDetailRepository.findItemsNotOrderedByProductId(id).Result;
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
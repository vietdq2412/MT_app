using MT_app.core.Models;
using MT_app.Infrastructure.Repository;

namespace MT_app.business.Services
{
    public interface ICustomerService : IBaseService<Customer>
    {
        bool CheckDuplicatePhoneNumber(string customerPhoneNumber);
        List<Customer> GetCustomersByContainPhoneNumber(string phoneNumber);
    }

    public class CustomerService : ICustomerService
    {
        public ICustomerRepository customerRepository { get; }

        public CustomerService(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task Save(Customer customer)
        {
            await customerRepository.Add(customer);
        }

        public async Task<List<Customer>> FindAll()
        {
            List<Customer> list = (await customerRepository.FindAll())!;
            return list;
        }

        public Task<Customer?> FindById(long? id)
        {
            return customerRepository.FindById(id);
        }

        public Task Delete(long id)
        {
            return customerRepository.Delete(id);
        }

        public Task Update(Customer t)
        {
            throw new NotImplementedException();
        }


        public bool CheckDuplicatePhoneNumber(string customerPhoneNumber)
        {
            return customerRepository.CheckDuplicatePhoneNumber(customerPhoneNumber);
        }

        public List<Customer> GetCustomersByContainPhoneNumber(string phoneNumber)
        {
            return customerRepository.GetCustomersByContainPhoneNumber(phoneNumber);
        }
    }
}
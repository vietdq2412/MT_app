using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT_app.core.Models;
using MT_app.Infrastructure.Repository;

namespace MT_app.business.Services
{
    public interface ICustomerService : IBaseService<Customer>
    {
        Task<Customer> FindByPhoneNumber(string phoneNumber);
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


        public Task Update(long id)
        {
            return customerRepository.Update(customerRepository.FindById(id).Result!);
        }

        public Task FindByPhoneNumber(string phoneNumber)
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

        Task<Customer> ICustomerService.FindByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }
    }
}
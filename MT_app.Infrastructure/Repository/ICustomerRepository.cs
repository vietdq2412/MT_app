using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        bool CheckDuplicatePhoneNumber(string phoneNumber);
        List<Customer> GetCustomersByContainPhoneNumber(string phoneNumber);
    }

    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private ApplicationDbContext DbContext { get; }

        public CustomerRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.DbContext = appDbContext;
        }

        public bool CheckDuplicatePhoneNumber(string phoneNumber)
        {
             return DbContext.Customers.Any(c => c.PhoneNumber == phoneNumber);
        }

        public List<Customer> GetCustomersByContainPhoneNumber(string phoneNumber)
        {
            return DbContext.Customers
                .Where(c => c.PhoneNumber.Contains(phoneNumber))
                .OrderByDescending(c => c.OrderCount)
                .ToList();
        }
    }
}
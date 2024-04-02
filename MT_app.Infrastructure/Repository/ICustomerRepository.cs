using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT_app.core.Models;
using MT_app.Infrastructure.Data;

namespace MT_app.Infrastructure.Repository
{
    public interface ICustomerRepository: IBaseRepository<Customer>
    {
        
    }

    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private ApplicationDbContext DbContext { get; }

        public CustomerRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            this.DbContext = appDbContext;
        }
    }
}

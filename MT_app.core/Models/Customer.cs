using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT_app.core.Models
{
    public class Customer:BaseEntity
    {
        public String? Name { get; set; }
        public String? PhoneNumber { get; set; }
        public String? Address { get; set; }
        public String? Description { get; set; }
        public int? OrderCount { get; set;}
    }
}

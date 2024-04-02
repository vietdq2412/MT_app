using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT_app.core.Models
{
    public class Customer:BaseEntity
    {
        public string? Name { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public int? OrderCount { get; set;}
        public ICollection<Order>? Orders { get; set;}

    }
}

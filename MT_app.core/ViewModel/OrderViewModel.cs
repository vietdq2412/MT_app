using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT_app.core.Models;

namespace MT_app.core.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number.")]
        public string? CustomerPhone { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; } 
        public string?   Note { get; set; } 
    }
}

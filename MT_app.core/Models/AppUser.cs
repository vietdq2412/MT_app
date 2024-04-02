using Microsoft.AspNetCore.Identity;

namespace MT_app.core.Models
{
    public class AppUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}

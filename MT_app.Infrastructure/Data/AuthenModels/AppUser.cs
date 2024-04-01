using Microsoft.AspNetCore.Identity;
using MT_app.core.Models;

namespace MT_app.Infrastructure.Data.AuthenModels
{
    public class AppUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MT_app.core.Models;

namespace MT_app.business.Services
{
    public interface IIdentityRoleService
    {
    }

    public class IdentityRoleService : IIdentityRoleService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppUser> _roleManager;
        private readonly IConfiguration _confinguration;

        public IdentityRoleService(
            UserManager<AppUser> userManager,
            RoleManager<AppUser> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _confinguration = configuration;
        }
    }
}
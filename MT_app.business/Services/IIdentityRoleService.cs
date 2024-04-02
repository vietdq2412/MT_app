using Microsoft.AspNetCore.Identity;
using MT_app.Infrastructure.Data.AuthenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MT_app.core.Models;

namespace MT_app.business.Services
{
    public interface IIdentityRoleService
    {

    }

    public class IdentityRoleService:IIdentityRoleService
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

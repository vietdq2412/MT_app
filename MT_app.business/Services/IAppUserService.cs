using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MT_app.core.Models;

namespace MT_app.business.Services
{
    public interface IAppUserService : IBaseService<AppUser>
    {
        Task<AppUser> GetByUsernameAsync(string username);

        Task<AppUser> GetByUserLogin(ClaimsPrincipal user);
    }

    public class AppUserService : IAppUserService
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AppUserService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }


        public Task Save(AppUser t)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppUser>> FindAll()
        {
            throw new NotImplementedException();
        }

        public Task<AppUser?> FindById(long? id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task Update(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetByUsernameAsync(string username)
        {
           return await _userManager.FindByEmailAsync(username);
        }

        public async Task<AppUser> GetByUserLogin(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

    }
}
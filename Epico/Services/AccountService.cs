using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Epico.Entity;
using Epico.Entity.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Task = System.Threading.Tasks.Task;

namespace Epico.Services
{
    public class AccountService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public AccountService(IServiceProvider serviceProvider)
        {
            _roleManager = serviceProvider.GetService(typeof(RoleManager<IdentityRole<int>>)) 
                as RoleManager<IdentityRole<int>>;
            
            _userManager = serviceProvider.GetService(typeof(UserManager<User>)) 
                as UserManager<User>;
            
            _signInManager = serviceProvider.GetService(typeof(SignInManager<User>)) 
                as SignInManager<User>;
            
            _httpContextAccessor = serviceProvider.GetService(typeof(IHttpContextAccessor)) 
                as IHttpContextAccessor;
        }        

        public async Task<bool> Login(string username, string password, bool persistent = true)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            return (await _signInManager.PasswordSignInAsync(username, password, persistent, false)).Succeeded;
        }

        public async Task<bool> Register(string username, string position, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(position) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var user = new User
            {
                UserName = username,
                Email = null,
                Position = position
            };

            var result = (await _userManager.CreateAsync(user, password)).Succeeded;
            
            return result; 
        }
        
        public async Task Logout() => await _signInManager.SignOutAsync();

        public string CurrentUserId()
            => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public async Task<User> GetByUserName(string username)
            => await _userManager.FindByNameAsync(username);

        public async Task<bool> SetRole(User user, UserRole userRole)
        {
            if (user == null)
            {
                return false;
            }

            var userRoleStr = userRole.ToString();
            if (!await _roleManager.RoleExistsAsync(userRoleStr))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(userRoleStr));
            }

            return (await _userManager.AddToRoleAsync(user, userRoleStr)).Succeeded;
        }
    }
}
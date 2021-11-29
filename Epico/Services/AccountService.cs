using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Epico.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Task = System.Threading.Tasks.Task;

namespace Epico.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public AccountService(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetService(typeof(UserManager<User>)) as UserManager<User>;
            _signInManager = serviceProvider.GetService(typeof(SignInManager<User>)) as SignInManager<User>;
            _httpContextAccessor = serviceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
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
            if (string.IsNullOrEmpty(username)|| string.IsNullOrEmpty(position) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var user = new User
            {
                UserName = username,
                Email = null,
                Position = position
            };
            
            return (await _userManager.CreateAsync(user, password)).Succeeded;
        }
        
        public async Task Logout() => await _signInManager.SignOutAsync();
        public string CurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
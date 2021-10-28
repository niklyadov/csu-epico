using System.Threading.Tasks;
using Epico.Entity;
using Microsoft.AspNetCore.Identity;
using Task = System.Threading.Tasks.Task;

namespace Epico.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Login(string username, string password, bool persistent = true)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            return (await _signInManager.PasswordSignInAsync(username, password, persistent, false)).Succeeded;
        }
        
        public async Task<bool> Register(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            var user = new User
            {
                UserName = username,
                Email = null
            };
            
            return (await _userManager.CreateAsync(user, password)).Succeeded;
        }
        
        public async Task Logout() => await _signInManager.SignOutAsync();
    }
}
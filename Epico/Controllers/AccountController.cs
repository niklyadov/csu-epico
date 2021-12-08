using Epico.Entity;
using Epico.Views;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    public class AccountController : BaseController
    {

        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (AccountService.CurrentUserId() != null)
            {
                return RedirectToAction("Index", "Product");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid && await AccountService.Login(model.Username, model.Password))
            {
                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await AccountService.Logout();

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid && await AccountService.Register(model.Username, model.Position, model.Password))
            {
                await SetFirstUserAsManager();

                if (AccountService.CurrentUserId() != null)
                    return RedirectToAction("Index", "Team");

                return RedirectToAction("New", "Product");
            }

            return View();
        }

        private async System.Threading.Tasks.Task SetFirstUserAsManager()
        {
            var users = await UserService.GetAll();
            if (users.Count > 0)
            {
                // первый юзер = менеджер
                await AccountService.SetRole(users.First(),
                    users.Count == 1 ? UserRole.Manager : UserRole.Default); 
            }
        }
    }
}
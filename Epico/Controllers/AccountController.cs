using System;
using System.Linq;
using System.Threading.Tasks;
using Epico.Entity;
using Epico.Entity.DAL.Repository;
using Epico.Views;
using Microsoft.AspNetCore.Mvc;
using Task = Epico.Entity.Task;

namespace Epico.Controllers
{
    public class AccountController : BaseController
    {
        
        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (AccountService.CurrentUserId() != null)
            {
                return RedirectToAction("Index", "Product");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid && await AccountService.Login(model.Username, model.Password))
            {
                return RedirectToAction("Index", "Product");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await AccountService.Logout();

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid && await AccountService.Register(model.Username, model.Position, model.Password))
            {
                var users = await UserService.GetAll();
                if (users.Count  >0)
                {
                    await AccountService.SetRole(users.First(), 
                        users.Count == 1 ? UserRole.Manager : UserRole.Default); // первый юзер =манагер :D
                }
                
                if (AccountService.CurrentUserId() != null)
                {
                    return RedirectToAction("Index", "Team");
                }
                return RedirectToAction("New", "Product");
            }
            
            return View();
        }
    }
}
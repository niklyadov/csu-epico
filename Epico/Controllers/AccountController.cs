using System;
using System.Threading.Tasks;
using Epico.Views;
using Microsoft.AspNetCore.Mvc;

namespace Epico.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        public IActionResult Registration()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid && await AccountService.Login(model.Username, model.Password))
            {
                return RedirectToAction("Index", "Project");
            }

            return BadRequest();
        }
        
        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid && await AccountService.Register(model.Username, model.Password))
            {
                return RedirectToAction("New", "Project");
            }
            
            return BadRequest();
        }
    }
}
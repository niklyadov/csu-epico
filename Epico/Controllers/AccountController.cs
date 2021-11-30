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

            return BadRequest();
        }


        [HttpGet]
        public IActionResult Registration()
        {
            if (AccountService.CurrentUserId() != null)
            {
                return RedirectToAction("Index", "Product");
            }
            
            return View();
        }
        
        
        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid && await AccountService.Register(model.Username, model.Position, model.Password))
            {
                return RedirectToAction("New", "Product");
            }
            
            return BadRequest();
        }
    }
}
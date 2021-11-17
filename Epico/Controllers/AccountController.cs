using System.Threading.Tasks;
using Epico.Services;
using Epico.Views;
using Microsoft.AspNetCore.Mvc;

namespace Epico.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
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
            if (ModelState.IsValid && await _accountService.Login(model.Username, model.Password))
            {
                return RedirectToAction("Index", "Project");
            }

            return BadRequest();
        }
        
        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid && await _accountService.Register(model.Username, model.Password))
            {
                return RedirectToAction("New", "Project");
            }
            
            return BadRequest();
        }
    }
}
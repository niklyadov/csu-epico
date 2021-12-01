using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Epico.Entity;

namespace Epico.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(IServiceProvider serviceProvider):base(serviceProvider)
        {
        }
        public IActionResult Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> New(NewUserViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            var user = await UserService.Add(new User
            {
                UserName = model.FirstName
            }); 
            return Ok("Сотрудник добавлен");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int userId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            await UserService.Delete(userId);
            return Ok("Сотрудник удалён");
        }
    }
}

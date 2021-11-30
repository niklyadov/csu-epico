using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            
            var user = await UserService.AddUser(model.FirstName, model.LastName); 
            return Ok("Сотрудник добавлен");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            await UserService.DeleteUser(userId);
            return Ok("Сотрудник удалён");
        }
    }
}

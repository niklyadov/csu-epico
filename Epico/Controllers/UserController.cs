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
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(IServiceProvider serviceProvider)
        {
            _userService = serviceProvider.GetService(typeof(UserService)) as UserService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> New(NewUserViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            var user = await _userService.AddUser(model.FirstName, model.LastName);
            return Ok("Сотрудник добавлен");
        }

        public async Task<IActionResult> Delete(string userId)
        {
            await _userService.DeleteUser(userId);
            return Ok("Сотрудник удалён");
        }
    }
}

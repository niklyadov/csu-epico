using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly UserService _userService;
        public TaskController(IServiceProvider serviceProvider)
        {
            _taskService = serviceProvider.GetService(typeof(TaskService)) as TaskService;
            _userService = serviceProvider.GetService(typeof(UserService)) as UserService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            return View(new NewTaskViewModel 
            { 
                PosibleUsers = await _userService.GetUsersList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var team = new List<User>();
                await _taskService.AddTask(model.Name, model.Description, team, model.DeadLine);
            }
            return Ok("Задача создана");
        }
    }
}

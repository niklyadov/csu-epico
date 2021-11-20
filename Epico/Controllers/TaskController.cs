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
        public TaskController(IServiceProvider serviceProvider)
        {
            _taskService = serviceProvider.GetService(typeof(TaskService)) as TaskService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New([FromQuery] int projectId)
        {
            // заменить на базу
            var users = new List<User>
                {
                    new User{ Id = "1", UserName = "Юзер 1" },
                    new User{ Id = "2", UserName = "Юзер 2" },
                    new User{ Id = "3", UserName = "Юзер 3" },
                };
            return View(new NewTaskViewModel 
            {
                ProjectId = projectId,
                PosibleUsers = users
                
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест                
                await _taskService.AddTask(model.Name, model.Description, model.DeadLine);
            }
            return Ok("Задача создана");
        }
    }
}

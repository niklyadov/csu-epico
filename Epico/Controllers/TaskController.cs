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
        public async Task<IActionResult> New()
        {
            return View(new NewTaskViewModel 
            { 
                PosibleUsers = new List<User> // для теста норм работает. Прикрутить тут базу юзеров
                {
                    new User{ Id = "1", UserName = "Первый" },
                    new User{ Id = "2", UserName = "Второй" },
                    new User{ Id = "3", UserName = "Третий" },
                }
                
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

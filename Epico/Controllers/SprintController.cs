using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class SprintController : Controller
    {
        private readonly SprintService _sprintService;
        private readonly TaskService _taskService;
        public SprintController(IServiceProvider serviceProvider)
        {
            _sprintService = serviceProvider.GetService(typeof(SprintService)) as SprintService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            return View(new NewSprintViewModel
            { 
                PosibleTasks = new List<Entity.Task> //  для теста
                { new Entity.Task { Name = "задача1" }, new Entity.Task { Name = "задача2" } } // await _taskService.GetTasks()
            });
        }
        [HttpPost]
        public async Task<IActionResult> New(NewSprintViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест
                var task = new Entity.Task(); // _taskService.GetTask(model.TasksId);
                await _sprintService.AddSprint(model.Name, new List<Entity.Task> { task });
            }
            return Ok("Спринт создан");
        }
    }
}

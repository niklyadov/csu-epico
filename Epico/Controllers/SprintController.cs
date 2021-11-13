using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Epico.Controllers
{
    [Authorize]
    public class SprintController : Controller
    {
        private readonly ISprintService _sprintService;
        private readonly ITaskService _taskService;
        public SprintController(IServiceProvider serviceProvider)
        {
            _sprintService = serviceProvider.GetService(typeof(ISprintService)) as ISprintService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            return View(new NewSprintViewModel
            { 
                PosibleTasks = new List<Entity.Task> //  для теста
                { new Entity.Task { Name = "задача1" }, new Entity.Task { Name = "задача2" } } // _taskService.GetTasks()
            });
        }
        [HttpPost]
        public IActionResult New(NewSprintViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест
                var task = new Entity.Task(); // _taskService.GetTask(model.TasksId);
                _sprintService.AddSprint(model.Name, new List<Entity.Task> { task });
            }
            return Ok("Спринт создан");
        }
    }
}

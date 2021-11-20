using Epico.Entity;
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
        private readonly ProjectService _projectService;
        private readonly SprintService _sprintService;
        private readonly TaskService _taskService;
        public SprintController(IServiceProvider serviceProvider)
        {
            _projectService = serviceProvider.GetService(typeof(ProjectService)) as ProjectService;
            _sprintService = serviceProvider.GetService(typeof(SprintService)) as SprintService;
            _taskService = serviceProvider.GetService(typeof(TaskService)) as TaskService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New([FromQuery] int projectId)
        {
            //var project = await _projectService.GetProjectById(projectId);
            // заменить на базу
            var features = new List<Feature> 
            { 
                new Feature { Name = "фича 1", ID = 1 }, 
                new Feature { Name = "фича 2", ID = 2 },
                new Feature { Name = "фича 3", ID = 3 },
                new Feature { Name = "фича 4", ID = 4 },
                new Feature { Name = "фича 5", ID = 5 },
            };
            return View(new NewSprintViewModel
            {
                ProjectID = projectId,
                PosibleFeatures = features
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

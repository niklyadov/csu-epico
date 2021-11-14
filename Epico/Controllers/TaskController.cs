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
        private readonly FeatureService _featureService;
        public TaskController(IServiceProvider serviceProvider)
        {
            _taskService = serviceProvider.GetService(typeof(TaskService)) as TaskService;
            _featureService = serviceProvider.GetService(typeof(FeatureService)) as FeatureService;
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
                PosibleTeams = new List<Team> // Для теста
                { new Team { Name = "фича1" }, new Team { Name = "фича2" } } // await _featureService.GetServices()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест
                var feature = new Feature(); // _featureService.GetFeature(model.FeaturesId);
                await _taskService.AddTask(model.Name, model.Description, new List<Feature> { feature }, model.DeadLine);
            }
            return Ok("Задача создана");
        }
    }
}

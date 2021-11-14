using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        public IActionResult New()
        {
            return View(new NewTaskViewModel 
            { 
                PosibleFeatures = new List<Feature> // Для теста
                { new Feature { Name = "фича1" }, new Feature { Name = "фича2" } } 
            });
        }

        [HttpPost]
        public IActionResult New(NewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест
                var feature = new Feature(); // _featureService.GetFeature(model.FeaturesId);
                _taskService.AddTask(model.Name, model.Description, new List<Feature> { feature }, model.DeadLine);
            }
            return Ok("Задача создана");
        }
    }
}

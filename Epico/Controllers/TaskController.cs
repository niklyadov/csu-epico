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
        private readonly ITaskService _taskService;
        private readonly IFeatureService _featureService;
        public TaskController(IServiceProvider serviceProvider)
        {
            _taskService = serviceProvider.GetService(typeof(ITaskService)) as ITaskService;
            _featureService = serviceProvider.GetService(typeof(IFeatureService)) as IFeatureService;
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
                _taskService.AddTask(model.Name, model.Description, new List<Feature> { model.Features }, model.DeadLine);
            }
            return Ok("Задача создана");
        }
    }
}

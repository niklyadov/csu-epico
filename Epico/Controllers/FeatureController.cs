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
    public class FeatureController : Controller
    {
        private readonly FeatureService _featureService;
        private readonly TaskService _taskService;
        private readonly MetricService _metricService;
        public FeatureController(IServiceProvider serviceProvider)
        {
            _featureService = serviceProvider.GetService(typeof(FeatureService)) as FeatureService;
            _taskService = serviceProvider.GetService(typeof(TaskService)) as TaskService;
            _metricService = serviceProvider.GetService(typeof(MetricService)) as MetricService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New([FromQuery] int projectId)
        {
            // заменить на базу
            var tasks = new List<Entity.Task>
            {
                new Entity.Task { Name = "задача 1", ID = 1 },
                new Entity.Task { Name = "задача 2", ID = 2 },
                new Entity.Task { Name = "задача 3", ID = 3 },
                new Entity.Task { Name = "задача 4", ID = 4 },
                new Entity.Task { Name = "задача 5", ID = 5 },
            };
            var metrics = new List<Metric>
            {
                new Metric { Name = "метрика 1" , ID = 1 },
                new Metric { Name = "метрика 2" , ID = 2 },
                new Metric { Name = "метрика 3" , ID = 3 },
                new Metric { Name = "метрика 4" , ID = 4 },
                new Metric { Name = "метрика 5" , ID = 5 },
            };
            return View(new NewFeatureViewModel
            {
                ProjectId = projectId,
                PosibleTasks = tasks,
                PosibleMetrics = metrics
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Вытаскивать задачи и метрики по Id из модели
                var tasks = new List<Entity.Task> 
                { 
                    new Entity.Task { Name = "задача 1" }, 
                    new Entity.Task { Name = "задача 2" } 
                };
                var metrics = new List<Metric> 
                { 
                    new Metric { Name = "Метрика 1" },
                    new Metric { Name = "Метрика 1" } 
                };

                await _featureService.AddFeature(model.Name, model.Description, model.Hypothesis, tasks, metrics);
            }
            return Ok("Фича добавлена");
        }
    }
}

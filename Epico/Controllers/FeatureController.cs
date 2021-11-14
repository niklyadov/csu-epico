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
        public async Task<IActionResult> New()
        {
            return View(new NewFeatureViewModel
            {
                PosibleTasks = new List<Entity.Task>  // для теста
                { new Entity.Task { Name = "команда1" }, new Entity.Task { Name = "команда2" } }, // await _teamService.GetTeams()
                PosibleMetrics = new List<Entity.Metric> // для теста
                { new Entity.Metric { Name = "метрика1" }, new Entity.Metric { Name = "метрика2" } } // await _metricService.GetMetrics()
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест
                var tasks = new List<Entity.Task> { new Entity.Task { Name = "задача 1" }, new Entity.Task { Name = "задача 2" } };
                // await _taskService.GetTasks();
                var metrics = new List<Metric> { new Metric { Name = "Метрика 1" }, new Metric { Name = "Метрика 1" } };
                // _metricService.GetMetrics(model.MetricId);
                await _featureService.AddFeature(model.Name, model.Description, model.Hypothesis, tasks, metrics);
            }
            return Ok("Фича добавлена");
        }
    }
}

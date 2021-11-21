using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return View(new NewFeatureViewModel
            {
                ProjectId = projectId,
                PosibleTasks = await _taskService.GetTaskList(),
                PosibleMetrics = await _metricService.GetMetricList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            var tasks = await _taskService.GetTaskListByIds(model.Tasks);
            var metrics = await _metricService.GetMetricListByIds(model.Metrics);

            await _featureService.AddFeature(model.Name, model.Description, model.Hypothesis, tasks, metrics);
            return Ok("Фича добавлена");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int projectId, [FromQuery] int featureId)
        {
            // todo прикрутить вытаскивание из базы для обновления
            var feature = new Feature
            {
                ID = featureId,
                Name = "фича 123 тест",
                Description = "Опиание фичи 123 тест",
                Hypothesis = "Гипотеза фичи 123 тест",
                State = FeatureState.NotStarted,
                Roadmap = RoadmapType.DoNow,
                Metric = new List<Metric>(),
                Tasks = new List<Entity.Task>()
            };
            return View(new EditFeatureViewModel
            {
                ID = feature.ID,
                Name = feature.Name,
                Description = feature.Description,
                Hypothesis = feature.Hypothesis,
                Metrics = feature.Metric.Select(x => x.ID).ToList(),
                Tasks = feature.Tasks.Select(x => x.ID).ToList(),
                Roadmap = feature.Roadmap,
                State = feature.State,

                ProjectId = projectId,
                PosibleTasks = await _taskService.GetTaskList(),
                PosibleMetrics = await _metricService.GetMetricList()
            }); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditFeatureViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            var tasks = await _taskService.GetTaskListByIds(model.Tasks);
            var metrics = await _metricService.GetMetricListByIds(model.Metrics);

            await _featureService.UpdateFeature(model.Name, model.Description, model.Hypothesis, tasks, metrics);
            return Ok("Фича изменена");
        }

        public async Task<IActionResult> Delete([FromQuery] int featureId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            // todo Прикрутить удаление фичи из базы
            await _featureService.DeleteFeature(featureId);
            return Ok("Фича удалена");
        }
    }
}

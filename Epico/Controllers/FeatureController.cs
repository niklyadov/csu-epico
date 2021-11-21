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
            var metrics = await _metricService.GetMetricListByIds(model.Tasks);

            await _featureService.AddFeature(model.Name, model.Description, model.Hypothesis, tasks, metrics);
            return Ok("Фича добавлена");
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

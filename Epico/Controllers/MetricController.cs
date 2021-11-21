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
    public class MetricController : Controller
    {

        private readonly ProjectService _projectService;
        private readonly AccountService _accountService;
        private readonly MetricService _metricService;

        public MetricController(IServiceProvider serviceProvider)
        {
            _projectService = serviceProvider.GetService(typeof(ProjectService)) as ProjectService;
            _accountService = serviceProvider.GetService(typeof(AccountService)) as AccountService;
            _metricService = serviceProvider.GetService(typeof(MetricService)) as MetricService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New([FromQuery] int projectId)
        {
            // todo заменить на вытаскивание из базы
            var metrics = new List<Metric> 
            {
                new Metric { Name = "метрика 1", ID = 1 }, 
                new Metric { Name = "метрика 2", ID = 2 },
                new Metric { Name = "метрика 3", ID = 3 },
                new Metric { Name = "метрика 4", ID = 4 },
                new Metric { Name = "метрика 5", ID = 5 },
            };
            return View(new NewMetricViewModel
            {
                ProjectId = projectId,
                PosibleParentMetrics = metrics
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewMetricViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            var metric = new Metric
            {
                Name = model.Name,
                Description = model.Description
            };
                
            if (model.PosibleParentMetrics != null && model.PosibleParentMetrics.Count != 0)
            {
                if (!model.ParentMetricId.HasValue) // Ошибка
                {
                    return BadRequest("ParentMetricId is not set");
                }
                    
                metric.ParentMetricId = model.ParentMetricId.Value;
            }

            var userOwner = _accountService.CurrentUserId();
            
            return Ok(await _projectService.AddMetric(userOwner, model.ProjectId, metric ));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int projectId, [FromQuery] int metricId)
        {
            // todo заменить на вытаскивание из базы
            var metric = new Metric
            {
                ID = 123,
                Name = "метрика 123 тест",
                Description = "описание метрики 123 тест",
                ParentMetricId = 122
            };
            // todo заменить на вытаскивание из базы
            var posibleParentMetrics = new List<Metric>
            {
                new Metric { Name = "метрика 1", ID = 1 },
                new Metric { Name = "метрика 2", ID = 2 },
                new Metric { Name = "метрика 3", ID = 3 },
                new Metric { Name = "метрика 4", ID = 4 },
                new Metric { Name = "метрика 5", ID = 5 },
            };
            return View(new EditMetricViewModel
            {
                ID = metric.ID,
                Name = metric.Name,
                Description = metric.Description,
                ParentMetricId = metric.ParentMetricId,
                ProjectId = projectId,
                PosibleParentMetrics = posibleParentMetrics
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMetricViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await _metricService.UpdateMetric(model.ID, model.Name, model.Description, model.ParentMetricId);
            return Ok("Метрика изменена");
        }

        public async Task<IActionResult> Delete([FromQuery] int metricId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            // todo Прикрутить удаление метрики из базы
            await _metricService.DeleteMetric(metricId);
            return Ok("Метрика удалена");
        }
    }
}

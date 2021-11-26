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
    public class MetricController : BaseController
    {
        public MetricController(IServiceProvider serviceProvider) :base(serviceProvider)
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New([FromQuery] int projectId)
        {
            return View(new NewMetricViewModel
            {
                ProjectId = projectId,
                PosibleParentMetrics = await MetricService.GetMetricList()
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
            return Ok(await MetricService.AddMetric(metric));
            //return Ok(await ProjectService.AddMetric(model.ProjectId, metric ));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int projectId, [FromQuery] int metricId)
        {
            var metric = await MetricService.GetMetricById(metricId);
            var possibleParentMetrics = await MetricService.GetMetricList();
            return View(new EditMetricViewModel
            {
                ID = metric.ID,
                Name = metric.Name,
                Description = metric.Description,
                ParentMetricId = metric.ParentMetricId,
                ProjectId = projectId,
                PosibleParentMetrics = possibleParentMetrics
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMetricViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await MetricService.UpdateMetric(new Metric()
            {
                ID = model.ID, 
                Name = model.Name, 
                Description = model.Description, 
                ParentMetricId = model.ParentMetricId
            });
            return Ok("Метрика изменена");
        }

        public async Task<IActionResult> Delete([FromQuery] int metricId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await MetricService.DeleteMetric(metricId);
            return Ok("Метрика удалена");
        }
    }
}

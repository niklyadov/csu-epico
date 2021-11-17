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
            var project = await _projectService.GetProjectById(projectId);
            return View(new NewMetricViewModel
            {
                AvailableParentMetrics = project.Metrics ??= new List<Metric>(),
                ProjectId = projectId
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
                
            if (model.AvailableParentMetrics != null && model.AvailableParentMetrics.Count != 0)
            {
                if (!model.ParentMetricId.HasValue) // Ошибка
                {
                    return BadRequest("ParentMetricId is not set");
                }
                    
                metric.ParentMetric = await _metricService.GetMetricById(model.ParentMetricId.Value);
            }

            var userOwner = _accountService.CurrentUserId();
            
            return Ok(await _projectService.AddMetric(userOwner, model.ProjectId, metric ));
        }
    }
}

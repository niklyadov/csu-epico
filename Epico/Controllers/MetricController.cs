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
    public class MetricController : Controller
    {

        private readonly IProjectService _projectService;
        private readonly IAccountService _accountService;
        private readonly IMetricService _metricService;

        public MetricController(IServiceProvider serviceProvider)
        {
            _projectService = serviceProvider.GetService(typeof(IProjectService)) as IProjectService;
            _accountService = serviceProvider.GetService(typeof(IAccountService)) as IAccountService;
            _metricService = serviceProvider.GetService(typeof(IMetricService)) as IMetricService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New(int id)
        {
            var project = await _projectService.GetProjectById(id);
            return View(new NewMetricViewModel
            {
                AvailableParentMetrics = (project.Metrics ??= new List<Metric>())
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(int id, NewMetricViewModel model)
        {
            if (ModelState.IsValid)
            {
                Metric metric;
                if (model.AvailableParentMetrics == null || model.AvailableParentMetrics.Count == 0)
                {
                    metric = await _metricService.AddMetric(model.Name, model.Description, null);
                }
                else
                {
                    if (!model.ParentMetricId.HasValue) // Ошибка
                    {
                        return BadRequest("Проблемы?");
                    }
                    metric = await _metricService.AddMetric(model.Name, model.Description, model.ParentMetricId.Value);

                }
                var project = await _projectService.GetProjectById(id);
                _projectService.AddMetric(_accountService.CurrentUserId(), id, metric);
                //(project.Metrics ??= new List<Metric>()).Add(metric);
            }
            return Ok("Работает :D");
        }
    }
}

using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Epico.Controllers
{
    [Authorize]
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;
        private readonly ITeamService _teamService;
        private readonly IMetricService _metricService;
        public FeatureController(IServiceProvider serviceProvider)
        {
            _featureService = serviceProvider.GetService(typeof(IFeatureService)) as IFeatureService;
            _teamService = serviceProvider.GetService(typeof(ITeamService)) as ITeamService;
            _metricService = serviceProvider.GetService(typeof(IMetricService)) as IMetricService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            return View(new NewFeatureViewModel
            {
                PosibleTeams = new List<Entity.Team>  // для теста
                { new Entity.Team { Name = "команда1" }, new Entity.Team { Name = "команда2" } }, //_teamService.GetTeams()
                PosibleMetrics = new List<Entity.Metric> // для теста
                { new Entity.Metric { Name = "метрика1" }, new Entity.Metric { Name = "метрика2" } } // _metricService.GetMetrics()
            });
        }
        
        [HttpPost]
        public IActionResult New(NewFeatureViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест
                var team = new Entity.Team();  // _teamService.GetTeam(model.TeamId);
                var metric = new Entity.Metric();  // _metricService.GetMetric(model.MetricId);
                _featureService.AddFeature(model.Name, model.Description, model.Hypothesis, team, metric);
            }
            return Ok("Фича добавлена");
        }
    }
}

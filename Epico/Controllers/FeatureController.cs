using Epico.Entity;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class FeatureController : BaseController
    {
        public FeatureController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<IActionResult> Index([FromQuery] bool metricError)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var features = await FeatureService.GetAll();
            return View(new FeatureViewModel
            {
                MetricError = metricError,
                Features = features
            });
        }

        [HttpGet]
        public async Task<IActionResult> New(FeatureViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetNewFeatureViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetNewFeatureViewModel());

            if (model.MetricId == 0) return RedirectToAction("Index", "Feature", new { metricError = true });

            var metric = await MetricService.GetById(model.MetricId);
            var users = await UserService.GetByIds(model.UserIds);

            await FeatureService.Add(new Feature
            {
                Name = model.Name,
                Description = model.Description,
                Hypothesis = model.Hypothesis,
                Tasks = new List<Entity.Task>(),
                Users = users,
                Metric = metric,
                Roadmap = model.Roadmap,
                State = FeatureState.Delivery
            });
            return RedirectToAction("Index", "Feature");
        }

        private async Task<NewFeatureViewModel> GetNewFeatureViewModel()
        {
            var possibleMetrics = await MetricService.GetAll();
            var posibleUsers = await UserService.GetAll();
            return new NewFeatureViewModel
            {
                PosibleUsers = posibleUsers,
                PosibleMetrics = possibleMetrics
            };
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int featureId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var feature = await FeatureService.GetById(featureId);
            return View(new EditFeatureViewModel
            {
                FeatureId = feature.ID,
                Name = feature.Name,
                Description = feature.Description,
                Hypothesis = feature.Hypothesis,
                MetricId = feature.Metric.ID,
                Tasks = feature.Tasks.Select(x => x.ID).ToList(),
                Roadmap = feature.Roadmap.Value,
                State = feature.State,

                PosibleTasks = await TaskService.GetAll(),
                PosibleMetrics = await MetricService.GetAll()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(new EditFeatureViewModel { FeatureId = model.FeatureId });

            var tasks = await TaskService.GetByIds(model.Tasks);
            var metrics = await MetricService.GetById(model.MetricId);
            var feature = await FeatureService.GetById(model.FeatureId);
            feature.Name = model.Name;
            feature.Description = model.Description;
            feature.Hypothesis = model.Hypothesis;
            feature.Metric = metrics;
            feature.Tasks = tasks;
            feature.State = model.State;
            feature.Roadmap = model.Roadmap;

            await FeatureService.Update(feature);
            return RedirectToAction("Index", "Feature");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] int featureId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await FeatureService.Delete(featureId);
            return RedirectToAction("Index", "Feature");
        }

        [HttpGet]
        public async Task<IActionResult> EditState(FeatureViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var feature = await FeatureService.GetById(model.FeatureId);
            return View(new EditStateFeatureViewModel
            {
                Feature = feature,
                FeatureId = feature.ID,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditState(EditStateFeatureViewModel model)
        {
            var feature = await FeatureService.GetById(model.FeatureId);
            feature.State = model.State;
            await FeatureService.Update(feature);
            return RedirectToAction("Index", "Feature");
        }
    }
}

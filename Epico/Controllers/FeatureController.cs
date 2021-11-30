using Epico.Entity;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> Index([FromQuery] bool taskError, [FromQuery] bool metricError)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            
            return View(new FeatureViewModel
            {
                TaskError = taskError,
                MetricError = metricError,
                ProductId = Product.ID,
                Features = await FeatureService.GetFeaturesList()
            });
        }

        [HttpGet]
        public async Task<IActionResult> New(FeatureViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var possibleMetrics = await MetricService.GetMetricList();
            var allTasks = await TaskService.GetTaskList();
            var possibleTasks = allTasks.Where(x => x.State != TaskState.Closed).ToList();

            return View(new NewFeatureViewModel
            {
                ProductId = model.ProductId,
                PosibleTasks = possibleTasks,
                PosibleMetrics = possibleMetrics
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool taskError = model.Tasks.Count == 0;
                bool metricError = model.Metrics.Count == 0;
                if (taskError || metricError)
                {
                    return RedirectToAction("Index", "Feature", new { taskError = taskError, metricError = metricError });
                }
                var tasks = await TaskService.GetTaskListByIds(model.Tasks);
                // todo переделать на одну метрику
                var metrics =  await MetricService.GetMetricListByIds(model.Metrics);

                await FeatureService.AddFeature(new Feature
                {
                    Name = model.Name,
                    Description = model.Description,
                    Hypothesis = model.Hypothesis,
                    Tasks = tasks,
                    Metric = metrics,
                    Roadmap = model.Roadmap,
                    State = FeatureState.NotStarted
                });
            }
            return RedirectToAction("Index", "Feature");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int projectId, [FromQuery] int featureId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var feature = await FeatureService.GetFeature(featureId);
            return View(new EditFeatureViewModel
            {
                ID = feature.ID,
                Name = feature.Name,
                Description = feature.Description,
                Hypothesis = feature.Hypothesis,
                Metrics = feature.Metric.Select(x => x.ID).ToList(),
                Tasks = feature.Tasks.Select(x => x.ID).ToList(),
                Roadmap = feature.Roadmap.Value,
                State = feature.State,

                ProductId = projectId,
                PosibleTasks = await TaskService.GetTaskList(),
                PosibleMetrics = await MetricService.GetMetricList()
            }); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditFeatureViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            var tasks = await TaskService.GetTaskListByIds(model.Tasks);
            var metrics = await MetricService.GetMetricListByIds(model.Metrics);

            await FeatureService.UpdateFeature(new Feature
            {
                ID = model.ID,
                Name = model.Name,
                Description = model.Description,
                Hypothesis = model.Hypothesis,
                Metric = metrics,
                Tasks = tasks,
                State = model.State
            });
            return RedirectToAction("Index", "Feature");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] int featureId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            await FeatureService.DeleteFeature(featureId);
            return RedirectToAction("Index", "Feature");
        }

        [HttpGet]
        public async Task<IActionResult> EditState(FeatureViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var feature = await FeatureService.GetFeatureById(model.FeatureId);
            return View(new EditStateFeatureViewModel
            {
                Feature = feature,
                FeatureId = feature.ID,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditState(EditStateFeatureViewModel model)
        {
            var feature = await FeatureService.GetFeatureById(model.FeatureId);
            feature.State = model.State;
            await FeatureService.UpdateFeature(feature);
            return RedirectToAction("Index", "Sprint");
        }
    }
}

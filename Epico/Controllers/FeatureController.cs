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
        public async Task<IActionResult> Index()
        {
            var product = await ProductService.GetProduct();
            if (product == null)
            {
                return RedirectToAction("New", "Product");
            }
            
            return View(new FeatureViewModel
            {
                ProductId = product.ID,
                Features = await FeatureService.GetFeaturesList()
            });
        }

        [HttpGet]
        public async Task<IActionResult> New(FeatureViewModel model)
        {
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
            return RedirectToAction("Show", "Product", new { id = model.ProductId });
        }

        public async Task<IActionResult> Delete([FromQuery] int featureId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            await FeatureService.DeleteFeature(featureId);
            return Ok("Фича удалена");
        }

        [HttpGet]
        public async Task<IActionResult> Add(SprintViewModel model)
        {
            var sprint = await SprintService.GetSprintById(model.SprintId);
            var features = await FeatureService.GetFeaturesList();
            return View(new AddFeatureViewModel
            {
                SprintName = sprint.Name,
                SprintId = sprint.ID,
                PossibleFeatures = features.Where(x => !sprint.Features.Contains(x)).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFeatureViewModel model)
        {
            // todo сохранить в базу новую фичу в спринте
            var sprint = await SprintService.GetSprintById(model.SprintId);
            sprint.Features.Add(await FeatureService.GetFeature(model.FeatureId));
            await SprintService.UpdateSprint(sprint);
            return RedirectToAction("Index", "Sprint");
        }
    }
}

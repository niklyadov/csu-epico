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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New([FromQuery] int projectId)
        {
            var posibleMetrics = await MetricService.GetMetricList();
            var allTasks = await TaskService.GetTaskList();
            var posibleTasks = allTasks.Where(x => x.State != TaskState.Closed).ToList();

            //if (posibleTasks?.Count == 0 || posibleMetrics?.Count == 0)
            //    return RedirectToAction("View", "Project", new { id = projectId });
            return View(new NewFeatureViewModel
            {
                ProjectId = projectId,
                PosibleTasks = posibleTasks,
                PosibleMetrics = posibleMetrics
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tasks = await TaskService.GetTaskListByIds(model.Tasks);
                var metrics =  await MetricService.GetMetricListByIds(model.Metrics);

                await FeatureService.AddFeature(new Feature
                {
                    Name = model.Name,
                    Description = model.Description,
                    Hypothesis = model.Hypothesis,
                    Tasks = tasks,
                    Metric = metrics,
                    State = FeatureState.NotStarted
                });
                // todo тут должен передаваться id спринта а не  1
                //await SprintService.AddFeature(1, new Feature
                //{
                //    Name = model.Name,
                //    Description = model.Description,
                //    Hypothesis = model.Hypothesis,
                //    Tasks = tasks,
                //    Metric = metrics,
                //    State = FeatureState.NotStarted
                //});
            }
            return RedirectToAction("View", "Project", new { id = model.ProjectId });
            //return Ok("Фича добавлена");
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
                Roadmap = feature.Roadmap,
                State = feature.State,

                ProjectId = projectId,
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

            await FeatureService.UpdateFeature(new Feature()
            {
                ID = model.ID,
                Name = model.Name,
                Description = model.Description,
                Hypothesis = model.Hypothesis,
                Metric = metrics,
                Tasks = tasks,
                State = model.State
            });
            return RedirectToAction("View", "Project", new { id = model.ProjectId });
            //return Ok("Фича изменена");
        }

        public async Task<IActionResult> Delete([FromQuery] int featureId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            await FeatureService.DeleteFeature(featureId);
            return Ok("Фича удалена");
        }
    }
}

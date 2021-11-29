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
    public class SprintController : BaseController
    {
        public SprintController(IServiceProvider serviceProvider):base(serviceProvider)
        {
        }
        
        public async Task<IActionResult> Index()
        {
            var sprints = await SprintService.GetSprintList();
            return View(new SprintViewModel
            {
                ProductId = 1, // todo извлекать из базы
                Sprints = sprints
            });
        }

        [HttpGet]
        public async Task<IActionResult> New(SprintViewModel model)
        {
            var allFeatures = await FeatureService.GetFeaturesList();
            var possibleFeatures = allFeatures.Where(x => x.State != FeatureState.Closed)
                                             .ToList();
            // Нельзя создать спринт если в проекте нет ни одной фичи
            if (possibleFeatures?.Count == 0)
                return RedirectToAction("Index", "Sprint");

            return View(new NewSprintViewModel
            {
                ProductId = model.ProductId,
                PosibleFeatures = possibleFeatures
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewSprintViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            // todo камень 
            if (ModelState.IsValid)
            {
                var features = await FeatureService.GetFeaturesListByIds(model.Features);

                await ProductService.AddSprint(model.ProductId, new Sprint
                {
                    Features = features,
                    Name = model.Name
                });
            }
            return RedirectToAction("Index", "Sprint");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int projectId, [FromQuery] int sprintId)
        {
            var sprint = await SprintService.GetSprintById(sprintId);

            return View(new EditSprintViewModel
            {
                ID = sprint.ID,
                Name = sprint.Name,
                Features = sprint.Features.Select(x => x.ID).ToList(),
                ProductId = projectId,
                PosibleFeatures = await FeatureService.GetFeaturesList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSprintViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            var features = await FeatureService.GetFeaturesListByIds(model.Features);
            await SprintService.UpdateSprint(new Sprint
            {
                ID = model.ID, 
                Name = model.Name,
                Features = features
            });
            return RedirectToAction("View", "Project", new { id = model.ProductId });
        }

        public async Task<IActionResult> Delete([FromQuery] int sprintId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await SprintService.DeleteSprint(sprintId);
            return Ok("Спринт удалён");
        }

        [HttpGet]
        public async Task<IActionResult> AddFeature(SprintViewModel model)
        {
            var sprint = await SprintService.GetSprintById(model.SprintId);
            var features = await FeatureService.GetFeaturesList();
            return View(new AddFeatureToSprintViewModel
            {
                SprintName = sprint.Name,
                SprintId = sprint.ID,
                PosibleFeatures = features.Where(x => !sprint.Features.Contains(x)).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddFeature(AddFeatureToSprintViewModel model)
        {
            var features = await FeatureService.GetFeaturesListByIds(model.FeatureIds);
            var sprint = await SprintService.GetSprintById(model.SprintId);

            if (features == null)
            {
                return NotFound("Feature not found");
            }

            sprint.Features.AddRange(features);
            
            await SprintService.UpdateSprint(sprint);

            return RedirectToAction("Index");
        }
    }
}

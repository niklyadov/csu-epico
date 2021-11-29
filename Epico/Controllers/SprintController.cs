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
            var product = await ProductService.GetProduct();
            if (product == null)
            {
                return RedirectToAction("New", "Product");
            }

            // todo sprints[i].Features пустой список
            var sprints = await SprintService.GetSprintList();
            return View(new SprintViewModel
            {
                ProductId = product.ID,
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
            var features = await FeatureService.GetFeaturesListByIds(model.Features);

            await ProductService.AddSprint(model.ProductId, new Sprint
            {
                Features = features,
                Name = model.Name
            });
            return RedirectToAction("Index", "Sprint");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(SprintViewModel model)
        {
            var sprint = await SprintService.GetSprintById(model.SprintId);

            return View(new EditSprintViewModel
            {
                SprintId = sprint.ID,
                Name = sprint.Name,
                Features = sprint.Features.Select(x => x.ID).ToList(),
                ProductId = 1, // todo take from DB
                PosibleFeatures = await FeatureService.GetFeaturesList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSprintViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            // todo fix adding features
            var features = await FeatureService.GetFeaturesListByIds(model.Features);
            await SprintService.UpdateSprint(new Sprint
            {
                ID = model.SprintId, 
                Name = model.Name,
                Features = features
            });
            return RedirectToAction("Index", "Sprint");
        }

        public async Task<IActionResult> Delete([FromQuery] int sprintId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            // todo fix delete from DB
            await SprintService.DeleteSprint(sprintId);
            return RedirectToAction("Index", "Sprint");
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

            return RedirectToAction("Index", "Sprint");
        }
    }
}

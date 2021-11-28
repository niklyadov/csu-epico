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
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New([FromQuery] int projectId)
        {
            var allFeatures = await FeatureService.GetFeaturesList();
            var posibleFeatures = allFeatures.Where(x => x.State != FeatureState.Closed)
                                             .ToList();
            // Нельзя создать спринт если в проекте нет ни одной фичи
            if (posibleFeatures?.Count == 0)
                return RedirectToAction("View", "Project", new { id = projectId });

            return View(new NewSprintViewModel
            {
                ProjectId = projectId,
                PosibleFeatures = posibleFeatures
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewSprintViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            if (ModelState.IsValid)
            {
                var features = await FeatureService.GetFeaturesListByIds(model.Features);

                await ProjectService.AddSprint(model.ProjectId, new Sprint
                {
                    Features = features,
                    Name = model.Name
                });
            }
            return RedirectToAction("View", "Project", new { id = model.ProjectId });
            //return Ok("Спринт создан");
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
                ProjectId = projectId,
                PosibleFeatures = await FeatureService.GetFeaturesList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSprintViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            var features = await FeatureService.GetFeaturesListByIds(model.Features);
            await SprintService.UpdateSprint(new Sprint()
            {
                ID = model.ID, 
                Name = model.Name,
                Features = features
            });
            return RedirectToAction("View", "Project", new { id = model.ProjectId });
            //return Ok("Спринт изменён");
        }

        public async Task<IActionResult> Delete([FromQuery] int sprintId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await SprintService.DeleteSprint(sprintId);
            return Ok("Спринт удалён");
        }

        [HttpGet]
        public async Task<IActionResult> Add([FromQuery] int sprintId)
        {
            var sprint = await SprintService.GetSprintById(sprintId);
            var features = await FeatureService.GetFeaturesList();
            return View(new AddSprintViewModel
            {
                SprintName = sprint.Name,
                SprintId = sprintId,
                Features = features.Where(x => !sprint.Features.Contains(x)).ToList()
        });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSprintViewModel model)
        {
            var feature = await FeatureService.GetFeature(model.FeatureId);
            var sprint = await SprintService.GetSprintById(model.SprintId);

            if (feature == null)
            {
                return NotFound("Feature not found");
            }

            sprint.Features.Add(feature);
            
            await SprintService.UpdateSprint(sprint);

            return RedirectToAction("Index");
        }
    }
}

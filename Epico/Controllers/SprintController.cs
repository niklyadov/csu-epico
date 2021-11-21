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
            //var project = await _projectService.GetProjectById(projectId);
            return View(new NewSprintViewModel
            {
                ProjectID = projectId,
                PosibleFeatures =  await FeatureService.GetFeaturesList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewSprintViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            if (ModelState.IsValid)
            {
                var features = await FeatureService.GetFeaturesListByIds(model.Features);

                await ProjectService.AddSprint(model.ProjectID, new Sprint
                {
                    Features = features,
                    Name = model.Name
                });
            }
            
            return Ok("Спринт создан");
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
                ProjectID = projectId,
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
            return Ok("Спринт изменён");
        }

        public async Task<IActionResult> Delete([FromQuery] int sprintId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await SprintService.DeleteSprint(sprintId);
            return Ok("Спринт удалён");
        }
    }
}

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
                // todo прикрутить вытаскивание фич по айдишкам
                var features = new List<Feature> { new Feature() };

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
            // todo прикрутить вытаскивание из базы
            var sprint = new Sprint
            {
                ID = 123,
                Name = "Спринт 123",
                Features = new List<Feature>()
            };

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

            // todo прикрутить вытаскивание фич по айдишкам
            var features = new List<Feature> { new Feature() };
            await SprintService.UpdateSprint(model.ID, model.Name, features);
            return Ok("Спринт изменён");
        }

        public async Task<IActionResult> Delete([FromQuery] int sprintId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            // todo Прикрутить удаление спринта из базы
            await SprintService.DeleteSprint(sprintId);
            return Ok("Спринт удалён");
        }
    }
}

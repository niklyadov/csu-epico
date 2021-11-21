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
    public class SprintController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly SprintService _sprintService;
        private readonly TaskService _taskService; // todo поидее taskService выпилить надо, ибо у нас фичи в спринтах
        private readonly FeatureService _featureService;
        public SprintController(IServiceProvider serviceProvider)
        {
            _projectService = serviceProvider.GetService(typeof(ProjectService)) as ProjectService;
            _sprintService = serviceProvider.GetService(typeof(SprintService)) as SprintService;
            // todo поидее taskService выпилить надо, ибо у нас фичи в спринтах
            _taskService = serviceProvider.GetService(typeof(TaskService)) as TaskService;
            _featureService = serviceProvider.GetService(typeof(FeatureService)) as FeatureService;
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
                PosibleFeatures =  await _featureService.GetFeaturesList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewSprintViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            // todo прикрутить вытаскивание фич по айдишкам
            var features = new List<Feature> { new Feature() };
            await _sprintService.AddSprint(model.Name, features);
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
                PosibleFeatures = await _featureService.GetFeaturesList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSprintViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            // todo прикрутить вытаскивание фич по айдишкам
            var features = new List<Feature> { new Feature() };
            await _sprintService.UpdateSprint(model.ID, model.Name, features);
            return Ok("Спринт изменён");
        }

        public async Task<IActionResult> Delete([FromQuery] int sprintId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            // todo Прикрутить удаление спринта из базы
            await _sprintService.DeleteSprint(sprintId);
            return Ok("Спринт удалён");
        }
    }
}

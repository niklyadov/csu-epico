using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly AccountService _accountService;

        public ProjectController(IServiceProvider serviceProvider)
        {
            _projectService = serviceProvider.GetService(typeof(ProjectService)) as ProjectService;
            _accountService = serviceProvider.GetService(typeof(AccountService)) as AccountService;
        }

        [Route("[controller]")]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.UserProjects(_accountService.CurrentUserId());
            return View(new ListProjectsViewModel
            {
                Projects = projects
            });
        }
        [Route("[controller]/{id:int}")]
        public async Task<IActionResult> View(int id)
        {
            //return Ok(await _projectService.GetProjectById(id));
            var project = await _projectService.UserProject(_accountService.CurrentUserId(), id);

            // Это всё заменить на базу
            var tasks = new List<Entity.Task>
            {
                new Entity.Task { Name = "задача 1" },
                new Entity.Task { Name = "задача 2" }
            };
            var features = new List<Feature>
            {
                new Feature { Name = "фича 1" },
                new Feature { Name = "фича 2" }
            };
            var sprints = new List<Sprint>
            {
                new Sprint { Name = "спринт 1" },
                new Sprint { Name = "спринт 2" }
            };
            var metrics = new List<Metric>
            {
                new Metric { Name = "метрика 1", Description = "описание метрики 1" },
                new Metric { Name = "метрика 2", Description = "описание метрики 2" }
            };
            var users = new List<User>
            {
                new User { UserName = "юзер 1" },
                new User { UserName = "юзер 2" }
            };

            return View(new ProjectViewModel
            {
                ProjectId = project.ID,
                Name = project.Name,
                Mission = project.Mission,
                Vision = project.Vision,
                OwnerUserId = project.OwnerUserId,
                ProductFormula = project.ProductFormula,
                Tasks = tasks,
                Features = features,
                Sprints = sprints,
                Metrics = metrics,
                Users = users
            });
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(NewProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _projectService.AddProject(model.Name,
                    model.Vision, model.Mission, model.ProductFormula, _accountService.CurrentUserId());

                if (result != null)
                {
                    return RedirectToAction("View", "Project", new { id = result.ID });
                }
            }


            return Ok("Hello");
        }
    }
}
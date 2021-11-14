using System;
using System.Linq;
using System.Threading.Tasks;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.UserProjects(_accountService.CurrentUserId());
            return View(new ListProjectsViewModel
            {
                Projects = projects
            });
        }
        public async Task<IActionResult> View(int id)
        {
            //return Ok(await _projectService.GetProjectById(id));
            var project = await _projectService.UserProject(_accountService.CurrentUserId(), id);
            return View(new ProjectViewModel
            {
                CurrentProject = project
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
                    return RedirectToAction("View", "Project", new {id = result.ID});
                }
            }
            

            return Ok("Hello");
        }
    }
}
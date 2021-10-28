using System;
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
        private readonly IProjectService _projectService;
        private readonly IAccountService _accountService;
        
        public ProjectController(IServiceProvider serviceProvider)
        {
            _projectService = serviceProvider.GetService(typeof(IProjectService)) as IProjectService;
            _accountService = serviceProvider.GetService(typeof(IAccountService)) as IAccountService;
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
            return Ok(await _projectService.GetProjectById(id));
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
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
        
        public IActionResult Index()
        {
            return Ok(_projectService.UserProjects(_accountService.CurrentUserId()));
        }
        public IActionResult View(int id)
        {
            return Ok(_projectService.GetProjectById(id));
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
                    return RedirectToAction("View", "Project", result.ID.ToString());
                }
            }
            

            return Ok("Hello");
        }
    }
}
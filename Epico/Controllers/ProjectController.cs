using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        public ProjectController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Route("[controller]")]
        public async Task<IActionResult> Index()
        {
            var userProjectId = await ProjectService.UserProjectId(AccountService.CurrentUserId());
            if (!userProjectId.HasValue)
                return NotFound("This user does not have a project!");

            return RedirectToAction("View", "Project", new { id = userProjectId });
            //return View(await ProjectService.GetProjectById(userProjectId.Value));
        }
        [Route("[controller]/{id:int}")]
        public async Task<IActionResult> View(int id)
        {
            var userProjectId = await ProjectService.UserProjectId(AccountService.CurrentUserId());
            if (!userProjectId.HasValue)
                return NotFound("This user does not have a project!");
            
            var project = await ProjectService.GetProjectById(userProjectId.Value);
            //var sprints = project.Sprints;

            //var features = sprints?.SelectMany(x => x.Features).ToList();
            //var metrics = features?.SelectMany(x=> x.Metric).ToList();
            //var tasks = features?.SelectMany(x => x.Tasks).ToList();
            //var users = tasks?.SelectMany(x=> x.Team).ToList();
            var sprints = await SprintService.GetSprintList();
            var features = await FeatureService.GetFeaturesList();
            var metrics = await MetricService.GetMetricList();
            var tasks = await TaskService.GetTaskList();
            var users = await UserService.GetUsersList();

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
            // todo ревью
            if (ProjectService.NotHasProject())
            {
                return View();
            }
            return BadRequest("Продукт уже существует.");
        }

        [HttpPost]
        public async Task<IActionResult> New(NewProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await ProjectService.AddProject(model.Name,
                    model.Vision, model.Mission, model.ProductFormula, AccountService.CurrentUserId());

                if (result != null)
                {
                    return RedirectToAction("View", "Project", new {id = result.ID});
                }
            }
            return Ok("Hello");
        }

        public async Task<IActionResult> Delete(int projectId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            await ProjectService.DeleteProject(projectId);
            return Ok("Hello");
        }
    }
}
using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        public ProductController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Route("[controller]")]
        public async Task<IActionResult> Index()
        {
            var userProjectId = await ProductService.UserProductId(AccountService.CurrentUserId());
            if (!userProjectId.HasValue)
                return RedirectToAction("New");

            return RedirectToAction("View", "Project", new { id = userProjectId });
            //return View(await ProjectService.GetProjectById(userProjectId.Value));
        }
        [Route("[controller]/{id:int}")]
        public async Task<IActionResult> View(int id)
        {
            var userProjectId = await ProductService.UserProductId(AccountService.CurrentUserId());
            if (!userProjectId.HasValue)
                return NotFound("This user does not have a project!");
            
            var project = await ProductService.GetProductById(userProjectId.Value);
            var sprints = await SprintService.GetSprintList();
            var features = await FeatureService.GetFeaturesList();
            var metrics = await MetricService.GetMetricList();
            var tasks = await TaskService.GetTaskList();
            var users = await UserService.GetUsersList();

            return View(new ProductViewModel
            {
                ProductId = project.ID,
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
            if (!ProductService.NotHasProject())
            {
                return RedirectToAction("New");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(NewProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await ProductService.AddProduct(model.Name,
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
            
            await ProductService.DeleteProduct(projectId);
            return Ok("Hello");
        }
    }
}
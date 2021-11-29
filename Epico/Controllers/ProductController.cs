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

            return RedirectToAction("Show", "Product");
            //return View(await ProjectService.GetProjectById(userProjectId.Value));
        }

        public async Task<IActionResult> Show()
        {
            var userProductId = await ProductService.UserProductId(AccountService.CurrentUserId());
            if (!userProductId.HasValue)
                return NotFound("This user does not have a project!");
            
            var proudct = await ProductService.GetProductById(userProductId.Value);
            var sprints = await SprintService.GetSprintList();
            var features = await FeatureService.GetFeaturesList();
            var metrics = await MetricService.GetMetricList();
            var tasks = await TaskService.GetTaskList();
            var users = await UserService.GetUsersList();

            return View(new ProductViewModel
            {
                ProductId = proudct.ID,
                Name = proudct.Name,
                Mission = proudct.Mission,
                Vision = proudct.Vision,
                OwnerUserId = proudct.OwnerUserId,
                ProductFormula = proudct.ProductFormula,
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
            if (!ProductService.NotHasProduct())
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
                    return RedirectToAction("Index", "Product");
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
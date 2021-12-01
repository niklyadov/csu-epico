using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            if (!HasProduct) return RedirectToAction("New", "Product");
            //var userProjectId = await ProductService.UserProductId(AccountService.CurrentUserId());
            //if (!userProjectId.HasValue)
            //    return RedirectToAction("New");

            return RedirectToAction("Show", "Product");
        }

        public async Task<IActionResult> Show()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            //var userProductId = await ProductService.UserProductId(AccountService.CurrentUserId());
            //if (!userProductId.HasValue)
            //    return NotFound("This user does not have a project!");
            
            //var product = await ProductService.GetProductById(userProductId.Value);
            var product = Product;
            var sprints = await SprintService.GetSprintList();
            var features = await FeatureService.GetFeaturesList();
            var metrics = await MetricService.GetMetricList();
            var tasks = await TaskService.GetAll();
            var users = await UserService.GetUsersList();

            return View(new ProductViewModel
            {
                ProductId = product.ID,
                Name = product.Name,
                Mission = product.Mission,
                Vision = product.Vision,
                OwnerUserId = product.OwnerUserId,
                ProductFormula = product.ProductFormula,
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
            if (HasProduct) 
                return RedirectToAction("Show", "Product");

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
            return View();
        }

        public async Task<IActionResult> Delete(int projectId)
        {
            if (!ModelState.IsValid) 
                return BadRequest("ModelState is not Valid");
            
            await ProductService.DeleteProduct(projectId);
            return RedirectToAction("New");
        }
    }
}
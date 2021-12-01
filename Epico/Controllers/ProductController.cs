using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Epico.Entity;

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
            var sprints = await SprintService.GetAll();
            var features = await FeatureService.GetAll();
            var metrics = await MetricService.GetAll();
            var tasks = await TaskService.GetAll();
            var users = await UserService.GetAll();

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
                var result = await ProductService.Add(new Product
                {
                    Name = model.Name,
                    Vision = model.Vision,
                    Mission = model.Mission,
                    ProductFormula = model.ProductFormula,
                    OwnerUserId = AccountService.CurrentUserId(),
                    Sprints = new List<Sprint>()
                });

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
            
            await ProductService.Delete(projectId);
            return RedirectToAction("New");
        }
    }
}
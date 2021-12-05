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
        public IActionResult Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            return RedirectToAction("Show", "Product");
        }

        public IActionResult Show()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var product = Product;
            return View(new ProductViewModel
            {
                ProductId = product.ID,
                Name = product.Name,
                Mission = product.Mission,
                Vision = product.Vision,
                OwnerUserId = product.OwnerUserId,
                ProductFormula = product.ProductFormula
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult New()
        {
            if (HasProduct) 
                return RedirectToAction("Show", "Product");

            return View();
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> New(NewProductViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

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

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            if (!ModelState.IsValid) 
                return BadRequest("ModelState is not Valid");
            
            await ProductService.Delete(id);
            return RedirectToAction("New");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var product = await ProductService.GetById(id);
            return View(new NewProductViewModel
            {
                Id = product.ID,
                Name = product.Name,
                Vision = product.Vision,
                Mission = product.Mission,
                ProductFormula = product.ProductFormula
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Edit(NewProductViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var product = await ProductService.GetById(model.Id);
            product.Name = model.Name;
            product.Vision = model.Vision;
            product.Mission = model.Mission;
            product.ProductFormula = model.ProductFormula;

            await ProductService.Update(product);
            return RedirectToAction("Show");
        }
    }
}
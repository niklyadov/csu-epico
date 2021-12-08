using Epico.Entity;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        public ProductController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region Index
        [Route("[controller]")]
        public IActionResult Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            return RedirectToAction("Show");
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
        #endregion

        #region New

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult New()
        {
            if (HasProduct)
                return RedirectToAction("Show");

            return View();
        }

        [Authorize(Roles = "Manager")]
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
                    return RedirectToAction("Index");
            }
            return View();
        }

        #endregion

        #region Edit

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

        #endregion

        #region Delete

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            await ProductService.Delete(id);
            return RedirectToAction("New");
        }

        #endregion
    }
}
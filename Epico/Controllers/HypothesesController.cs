using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epico.Models;
using Epico.Services;

namespace Epico.Controllers
{
    public class HypothesesController : BaseController
    {
        public HypothesesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<IActionResult> Index()
        {
            var product = await ProductService.GetProduct();
            if (product == null)
            {
                return RedirectToAction("New", "Product");
            }
            return View(new FeatureViewModel
            {
                Features = await FeatureService.GetFeaturesList()
            });
        }
    }
}

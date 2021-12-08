using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class HypothesisController : FeatureControllerBase
    {
        public HypothesisController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var features = await FeatureService.GetAllHypotheses();
            return View(new HypothesisViewModel
            {
                Hypothesis = features
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public override async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetNewFeatureVM());

            var metric = await GetMetric(model.MetricId);
            var users = await UserService.GetByIds(model.UserIds);

            await FeatureService.Add(new Feature
            {
                Name = model.Name,
                Description = model.Description,
                Tasks = new List<Entity.Task>(),
                Users = users,
                Metric = metric,
                IsFeature = false
            });
            return RedirectToAction("Index");
        }
    }
}

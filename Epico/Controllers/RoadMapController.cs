using Epico.Models;
using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Epico.Controllers
{

    [Authorize]
    public class RoadMapController : BaseController
    {
        public RoadMapController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region Index

        public async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var features = await FeatureService.GetAllFeatures();
            return View(new RoadMapViewModel
            {
                Features = features
            });
        }

        #endregion

        #region AddFeature

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> AddFeature(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var features = await FeatureService.GetAllFeatures();
            var query = features.Where(x => x.Roadmap != (RoadmapType)id)
                                .Where(x => x.State != FeatureState.None && x.State != FeatureState.Rejected);

            return View(new AddRoadmapViewModel
            {
                Roadmap = (RoadmapType)id,
                Features = query.ToList()
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AddFeature(AddRoadmapViewModel model)
        {
            var feature = await FeatureService.GetById(model.FeatureId);

            if (feature == null)
            {
                return NotFound("Feature not found");
            }

            feature.Roadmap = model.Roadmap;
            await FeatureService.Update(feature);

            return RedirectToAction("Index");
        }

        #endregion
    }
}

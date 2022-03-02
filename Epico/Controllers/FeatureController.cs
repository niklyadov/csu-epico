using Epico.Models.ProductEntity.Feature;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class FeatureController : ProductEntityController
    {
        public FeatureController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var model = await IndexPreparation(true);
            return View(model);
        }

        #endregion

        #region New

        [HttpGet]
        public async Task<IActionResult> New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var model = (NewFeatureViewModel)await NewPreparationGet(true);
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(await NewPreparationGet(true));

            await NewPreparationPost(model, true);
            return RedirectToAction("Index", "Feature");
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var model = (EditFeatureViewModel)await EditPreparationGet(id, true);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditFeatureViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(await EditPreparationGet(model.Id, true));

            await EditPreparationPost(model, true);
            return RedirectToAction("Index");
        }

        #endregion

        #region EditState

        [HttpGet]
        public async Task<IActionResult> EditState(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var feature = await FeatureService.GetById(id);
            return View(new EditStateFeatureViewModel
            {
                Feature = feature,
                Id = feature.ID,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditState(EditStateFeatureViewModel model)
        {
            var feature = await FeatureService.GetById(model.Id);
            feature.State = model.State;
            await FeatureService.Update(feature);
            return RedirectToAction("Index");
        }

        #endregion

    }
}

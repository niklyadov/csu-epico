using Epico.Models.ProductEntity.Hypothesis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class HypothesisController : ProductEntityController
    {
        public HypothesisController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var model = await IndexPreparation(false);
            return View(model);
        }

        #endregion

        #region New

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var model = (NewHypothesisViewModel)await NewPreparationGet(false);
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> New(NewHypothesisViewModel model)
        {
            if (!ModelState.IsValid) return View(await NewPreparationGet(false));

            await NewPreparationPost(model, false);
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var model = (EditHypothesisViewModel)await EditPreparationGet(id, false);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditHypothesisViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(await EditPreparationGet(model.Id, false));

            await EditPreparationPost(model, false);
            return RedirectToAction("Index");
        }

        #endregion

    }
}

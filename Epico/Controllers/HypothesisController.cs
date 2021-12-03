using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Epico.Entity;

namespace Epico.Controllers
{
    [Authorize]
    public class HypothesisController : BaseController
    {
        public HypothesisController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            // todo переделать на гипотезы
            var features = await FeatureService.GetAllHypotheses();
            return View(new HypothesisViewModel
            {
                Hypothisis = features
            });
        }

        [HttpGet]
        public IActionResult New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(NewHypothesisViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(NewHypothesisViewModel model)
        {
            if (!ModelState.IsValid) return View(NewHypothesisViewModel());

            if (model.MetricId == 0) 
                return RedirectToAction("Index", new { metricError = true });

            var metric = await MetricService.GetById(model.MetricId);
            var users = await UserService.GetByIds(model.UserIds);

            // todo переделать на гипотезы
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

        private NewHypothesisViewModel NewHypothesisViewModel()
        {
            var possibleMetrics = MetricService.GetAll().Result;
            var posibleUsers = UserService.GetAll().Result;
            return new NewHypothesisViewModel
            {
                PosibleUsers = posibleUsers,
                PosibleMetrics = possibleMetrics
            };
        }


        [HttpGet]
        public IActionResult Edit([FromQuery] int hypothisisId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(GetEditHypothesisViewModel(hypothisisId));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditHypothesisViewModel model)
        {
            if (!ModelState.IsValid) return View(GetEditHypothesisViewModel(model.HypothesisId));

            var users = await UserService.GetByIds(model.UserIds);
            var metrics = await MetricService.GetById(model.MetricId);
            
            // todo переделать на гипотезы
            var hypothesis = await FeatureService.GetById(model.HypothesisId);
            hypothesis.Name = model.Name;
            hypothesis.Description = model.Description;
            hypothesis.Metric = metrics;
            hypothesis.Users = users;
            // todo переделать на гипотезы
            await FeatureService.Update(hypothesis);
            return RedirectToAction("Index");
        }

        private EditHypothesisViewModel GetEditHypothesisViewModel(int hypothesisId)
        {
            var hypothesis = FeatureService.GetById(hypothesisId).Result;
            return new EditHypothesisViewModel
            {
                HypothesisId = hypothesis.ID,
                Name = hypothesis.Name,
                Description = hypothesis.Description,
                MetricId = hypothesis.Metric.ID,
                UserIds = hypothesis.Users.Select(x => x.Id).ToList(),

                PosibleUsers = UserService.GetAll().Result,
                PosibleMetrics = MetricService.GetAll().Result
            };
        }


        [HttpGet]
        public async Task<IActionResult> NewTask([FromQuery] int hypothisisId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            var hypothisis = await FeatureService.GetById(hypothisisId);
            return View(new NewTaskByIdViewModel
            {
                FeatureId = hypothisisId,
                PossibleUsers = (await UserService.GetAll())
                                .Where(user => hypothisis.Users.Contains(user))
                                .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> NewTask(NewTaskByIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var rehypothisis = await FeatureService.GetById(model.FeatureId);
                return View(new NewTaskByIdViewModel
                {
                    FeatureId = model.FeatureId,
                    PossibleUsers = (await UserService.GetAll())
                                .Where(user => rehypothisis.Users.Contains(user))
                                .ToList()
                });
            }

            if (model.UserId == 0)
            {
                return RedirectToAction("Index", "Feature", new { taskCreateError = true });
            }

            var hypothisis = await FeatureService.GetById(model.FeatureId);
            if (!hypothisis.Users.Select(x => x.Id).Contains(model.UserId))
            {
                return BadRequest("Юзер не доступен т.к. не содержится в команде гипотезы. Вы чайник.");
            }

            var responsibleUser = await UserService.GetById(model.UserId);
            var task = await TaskService.Add(new Entity.Task
            {
                Name = model.Name,
                Description = model.Description,
                DeadLine = model.DeadLine,
                ResponsibleUser = responsibleUser
            });
            hypothisis.Tasks.Add(task);
            await FeatureService.Update(hypothisis);
            return RedirectToAction("Index", "Feature");
        }
    }
}

using Epico.Entity;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class FeatureController : BaseController
    {
        public FeatureController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<IActionResult> Index([FromQuery] bool metricError)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var features = await FeatureService.GetAllFeatures();
            return View(new FeatureViewModel
            {
                MetricError = metricError,
                Features = features
            });
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetNewFeatureViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetNewFeatureViewModel());

            if (model.MetricId == 0) return RedirectToAction("Index", "Feature", new { metricError = true });

            var metric = await MetricService.GetById(model.MetricId);
            var users = await UserService.GetByIds(model.UserIds);

            await FeatureService.Add(new Feature
            {
                Name = model.Name,
                Description = model.Description,
                //Hypothesis = model.Hypothesis,
                Tasks = new List<Entity.Task>(),
                Users = users,
                Metric = metric,
                Roadmap = model.Roadmap,
                State = FeatureState.Discovery,
                IsFeature = true
            });
            return RedirectToAction("Index", "Feature");
        }

        private async Task<NewFeatureViewModel> GetNewFeatureViewModel()
        {
            var possibleMetrics = await MetricService.GetAll();
            var posibleUsers = await UserService.GetAll();
            return new NewFeatureViewModel
            {
                PosibleUsers = posibleUsers,
                PosibleMetrics = possibleMetrics
            };
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(GetEditFeatureViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(GetEditFeatureViewModel(model.FeatureId));

            var users = await UserService.GetByIds(model.UserIds);
            var metrics = await MetricService.GetById(model.MetricId);
            var feature = await FeatureService.GetById(model.FeatureId);
            feature.Name = model.Name;
            feature.Description = model.Description;
            //feature.Hypothesis = model.Hypothesis;
            feature.Metric = metrics;
            feature.Users = users;
            feature.State = model.State;
            feature.Roadmap = model.Roadmap;

            await FeatureService.Update(feature);
            return RedirectToAction("Index");
        }

        private EditFeatureViewModel GetEditFeatureViewModel(int featureId)
        {
            var feature = FeatureService.GetById(featureId).Result;
            return new EditFeatureViewModel
            {
                FeatureId = feature.ID,
                Name = feature.Name,
                Description = feature.Description,
               // Hypothesis = feature.Hypothesis,
                MetricId = feature.Metric.ID,
                UserIds = feature.Users.Select(x => x.Id).ToList(),
                Roadmap = feature.Roadmap.Value,
                State = feature.State,

                PosibleUsers = UserService.GetAll().Result,
                PosibleMetrics = MetricService.GetAll().Result
            };
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await FeatureService.Delete(id);
            return RedirectToAction("Index", "Feature");
        }

        [HttpGet]
        public async Task<IActionResult> EditState(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var feature = await FeatureService.GetById(id);
            return View(new EditStateFeatureViewModel
            {
                Feature = feature,
                FeatureId = feature.ID,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditState(EditStateFeatureViewModel model)
        {
            var feature = await FeatureService.GetById(model.FeatureId);
            feature.State = model.State;
            await FeatureService.Update(feature);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> NewTask(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            var feature = await FeatureService.GetById(id);
            return View(new NewTaskByIdViewModel
            {
                FeatureId = feature.ID,
                PossibleUsers = (await UserService.GetAll())
                                .Where(user => feature.Users.Contains(user))
                                .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> NewTask(NewTaskByIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var refeature = await FeatureService.GetById(model.FeatureId);
                return View(new NewTaskByIdViewModel
                {
                    FeatureId = model.FeatureId,
                    PossibleUsers = (await UserService.GetAll())
                                .Where(user => refeature.Users.Contains(user))
                                .ToList()
                });
            }

            if (model.UserId == 0)
            {
                return RedirectToAction("Index", "Feature", new { taskCreateError = true });
            }

            var feature = await FeatureService.GetById(model.FeatureId);
            if (!feature.Users.Select(x => x.Id).Contains(model.UserId))
            {
                return BadRequest("Юзер не доступен т.к. не содержится в команде фичи. Вы чайник.");
            }

            var responsibleUser = await UserService.GetById(model.UserId);
            var task = await TaskService.Add(new Entity.Task
            {
                Name = model.Name,
                Description = model.Description,
                DeadLine = model.DeadLine,
                ResponsibleUser = responsibleUser
            });
            feature.Tasks.Add(task);
            await FeatureService.Update(feature);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTask(int id, int taskId)
        {
            var task = await TaskService.GetById(taskId);
            if (task == null)
                return BadRequest("Задача не найдена.");

            var feature = await FeatureService.GetById(id);
            if (!feature.Tasks.Contains(task))
                return BadRequest("Фича не содержит эту задачу.");

            feature.Tasks.Remove(task);
            await FeatureService.Update(feature);
            await TaskService.Delete(task.ID);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditTask(int id, int taskId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetEditTaskViewModel(id, taskId));
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(EditTaskViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetEditTaskViewModel(model.FeatureId, model.TaskId));

            var responsibleUser = await UserService.GetById(model.UserId);
            var task = await TaskService.GetById(model.TaskId);
            task.Name = model.Name;
            task.Description = model.Description;
            task.ResponsibleUser = responsibleUser;
            task.DeadLine = model.DeadLine;
            task.State = (TaskState)model.State;

            await TaskService.Update(task);

            return RedirectToAction("Index");
        }

        private async Task<EditTaskViewModel> GetEditTaskViewModel(int featureId, int taskId)
        {
            var task = await TaskService.GetById(taskId);
            var feature = await FeatureService.GetById(featureId);
            var posibleUsers = (await UserService.GetAll())
                               .Where(user => feature.Users.Contains(user))
                               .ToList();
            return new EditTaskViewModel
            {
                TaskId = task.ID,
                FeatureId = feature.ID,
                Name = task.Name,
                Description = task.Description,
                DeadLine = task.DeadLine,
                State = (int)task.State,
                UserId = task.ResponsibleUser != null ? task.ResponsibleUser.Id : 0,
                PosibleUsers = posibleUsers
            };
        }
    }
}

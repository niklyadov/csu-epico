using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            var features = await FeatureService.GetAllHypotheses();
            return View(new HypothesisViewModel
            {
                Hypothesis = features
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

            Metric metric = null;
            if (model.MetricId != 0)
                metric = await MetricService.GetById(model.MetricId);

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
        public IActionResult Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(GetEditHypothesisViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditHypothesisViewModel model)
        {
            if (!ModelState.IsValid) return View(GetEditHypothesisViewModel(model.HypothesisId));

            var users = await UserService.GetByIds(model.UserIds);
            Metric metric = null;
            if (model.MetricId != 0)
                metric = await MetricService.GetById(model.MetricId);

            var hypothesis = await FeatureService.GetById(model.HypothesisId);
            // Отвязка ответственного если его убрали из команды
            foreach (var item in hypothesis.Tasks)
                if (item.ResponsibleUserId.HasValue)
                    if (!model.UserIds.Contains(item.ResponsibleUserId.Value))
                        item.ResponsibleUser = null;

            // todo переделать на гипотезы
            hypothesis.Name = model.Name;
            hypothesis.Description = model.Description;
            hypothesis.Metric = metric;
            hypothesis.Users = users;
            // todo переделать на гипотезы
            await FeatureService.Update(hypothesis);
            return RedirectToAction("Index");
        }

        private EditHypothesisViewModel GetEditHypothesisViewModel(int hypothesisId)
        {
            var hypothesis = FeatureService.GetById(hypothesisId).Result;
            var metricId = hypothesis.Metric != null ? hypothesis.Metric.ID : 0;
            return new EditHypothesisViewModel
            {
                HypothesisId = hypothesis.ID,
                Name = hypothesis.Name,
                Description = hypothesis.Description,
                MetricId = metricId,
                UserIds = hypothesis.Users.Select(x => x.Id).ToList(),

                PosibleUsers = UserService.GetAll().Result,
                PosibleMetrics = MetricService.GetAll().Result
            };
        }


        [HttpGet]
        public async Task<IActionResult> NewTask(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            var hypothesis = await FeatureService.GetById(id);
            return View(new NewTaskByIdViewModel
            {
                FeatureId = hypothesis.ID,
                PossibleUsers = (await UserService.GetAll())
                                .Where(user => hypothesis.Users.Contains(user))
                                .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> NewTask(NewTaskByIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var feature = await FeatureService.GetById(model.FeatureId);
                return View(new NewTaskByIdViewModel
                {
                    FeatureId = model.FeatureId,
                    PossibleUsers = (await UserService.GetAll())
                                .Where(user => feature.Users.Contains(user))
                                .ToList()
                });
            }

            if (model.UserId == 0)
            {
                return RedirectToAction("Index", "Feature", new { taskCreateError = true });
            }

            var hypothesis = await FeatureService.GetById(model.FeatureId);
            if (!hypothesis.Users.Select(x => x.Id).Contains(model.UserId))
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
            hypothesis.Tasks.Add(task);
            await FeatureService.Update(hypothesis);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTask(int id, int taskId)
        {
            var task = await TaskService.GetById(taskId);
            if (task == null)
                return BadRequest("Задача не найдена.");

            var hypothesis = await FeatureService.GetById(id);
            if (!hypothesis.Tasks.Contains(task))
                return BadRequest("Гипотеза не содержит эту задачу.");

            hypothesis.Tasks.Remove(task);
            await FeatureService.Update(hypothesis);
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

        private async Task<EditTaskViewModel> GetEditTaskViewModel(int hypothesisId, int taskId)
        {
            var task = await TaskService.GetById(taskId);
            var hypothesis = await FeatureService.GetById(hypothesisId);
            var posibleUsers = (await UserService.GetAll())
                               .Where(user => hypothesis.Users.Contains(user))
                               .ToList();
            return new EditTaskViewModel
            {
                TaskId = task.ID,
                FeatureId = hypothesis.ID,
                Name = task.Name,
                Description = task.Description,
                DeadLine = task.DeadLine,
                State = (int)task.State,
                UserId = task.ResponsibleUser != null ? task.ResponsibleUser.Id : 0,
                PosibleUsers = posibleUsers
            };
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var hypothesis = await FeatureService.GetById(id);
            var sprints = (await SprintService.GetAll())
               .Where(sprint => sprint.Features.Contains(hypothesis))
               .ToList();
            sprints.ForEach(sprint => sprint.Features.Remove(hypothesis));

            foreach (var item in sprints)
                await SprintService.Update(item);

            foreach (var item in hypothesis.Tasks)
                TaskService.Delete(item.ID);

            await FeatureService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

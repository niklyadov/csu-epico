using Epico.Entity;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    public class FeatureControllerBase : BaseController
    {
        public FeatureControllerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region Index

        public virtual async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var features = await FeatureService.GetAllFeatures();
            return View(new FeatureViewModel
            {
                Features = features
            });
        }

        #endregion

        #region New

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public virtual async Task<IActionResult> New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetNewFeatureVM());
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public virtual async Task<IActionResult> New(NewFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetNewFeatureVM());

            var metric = await GetMetric(model.MetricId);
            var users = await UserService.GetByIds(model.UserIds);

            await FeatureService.Add(new Feature
            {
                Name = model.Name,
                Description = model.Description,
                Tasks = new(),
                Users = users,
                Metric = metric,
                Roadmap = model.Roadmap,
                State = FeatureState.Discovery,
                IsFeature = true
            });
            return RedirectToAction("Index");
        }

        protected async Task<Metric> GetMetric(int id) => id != 0 ? await MetricService.GetById(id) : null;

        protected async Task<NewFeatureViewModel> GetNewFeatureVM()
        {
            var possibleMetrics = await MetricService.GetAll();
            var posibleUsers = await UserService.GetAll();
            return new NewFeatureViewModel
            {
                PosibleUsers = posibleUsers,
                PosibleMetrics = possibleMetrics
            };
        }

        #endregion

        #region Edit

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public virtual async Task<IActionResult> Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetEditFeatureViewModel(id));
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public virtual async Task<IActionResult> Edit(EditFeatureViewModel model)
        {
            if (!ModelState.IsValid) return View(GetEditFeatureViewModel(model.FeatureId));

            var users = await UserService.GetByIds(model.UserIds);

            var metric = await GetMetric(model.MetricId);
            var feature = await FeatureService.GetById(model.FeatureId);

            // Отвязка ответственного если его убрали из команды
            foreach (var item in feature.Tasks)
                if (item.ResponsibleUserId.HasValue)
                    if (!model.UserIds.Contains(item.ResponsibleUserId.Value))
                        item.ResponsibleUser = null;

            feature.Name = model.Name;
            feature.Description = model.Description;
            feature.Metric = metric;
            feature.Users = users;
            feature.State = model.State;
            feature.Roadmap = model.Roadmap;

            await FeatureService.Update(feature);
            return RedirectToAction("Index");
        }

        private async Task<EditFeatureViewModel> GetEditFeatureViewModel(int featureId)
        {
            var feature = await FeatureService.GetById(featureId);
            var metricId = feature.Metric != null ? feature.Metric.ID : 0;

            var posibleUsers = await UserService.GetAll();
            var posibleMetrics = await MetricService.GetAll();
            var roadmap = feature.Roadmap != null ? feature.Roadmap.Value : 0;
            return new EditFeatureViewModel
            {
                FeatureId = feature.ID,
                Name = feature.Name,
                Description = feature.Description,
                MetricId = metricId,
                UserIds = feature.Users.Select(x => x.Id).ToList(),
                Roadmap = roadmap,
                State = feature.State,
                PosibleUsers = posibleUsers,
                PosibleMetrics = posibleMetrics
            };
        }

        #endregion

        #region EditState

        [Authorize(Roles = "Manager")]
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

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> EditState(EditStateFeatureViewModel model)
        {
            var feature = await FeatureService.GetById(model.FeatureId);
            feature.State = model.State;
            await FeatureService.Update(feature);
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var feature = await FeatureService.GetById(id);
            var sprints = (await SprintService.GetAll())
               .Where(sprint => sprint.Features.Contains(feature))
               .ToList();
            sprints.ForEach(sprint => sprint.Features.Remove(feature));

            await SprintService.UpdateRange(sprints);
            await TaskService.DeleteRange(feature.Tasks);
            await FeatureService.Delete(id);
            return RedirectToAction("Index");
        }

        #endregion

        #region NewTask

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> NewTask(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            return View(await GetNewTaskVM(id));
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> NewTask(NewTaskByIdViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await GetNewTaskVM(model.FeatureId));

            if (model.UserId == 0)
                return RedirectToAction("Index", new { taskCreateError = true });

            var feature = await FeatureService.GetById(model.FeatureId);
            if (!feature.Users.Select(x => x.Id).Contains(model.UserId))
                return BadRequest("Юзер не доступен т.к. не содержится в команде фичи/гипотеза");

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

        public async Task<NewTaskByIdViewModel> GetNewTaskVM(int id)
        {
            var feature = await FeatureService.GetById(id);
            var posibleUsers = (await UserService.GetAll())
                                .Where(user => feature.Users.Contains(user))
                                .ToList();
            return new NewTaskByIdViewModel
            {
                FeatureId = feature.ID,
                PossibleUsers = posibleUsers
            };
        }

        #endregion

        #region EditTask

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> EditTask(int id, int taskId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetEditTaskVM(id, taskId));
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> EditTask(EditTaskViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await GetEditTaskVM(model.FeatureId, model.TaskId));

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

        private async Task<EditTaskViewModel> GetEditTaskVM(int featureId, int taskId)
        {
            var task = await TaskService.GetById(taskId);
            var feature = await FeatureService.GetById(featureId);
            var posibleUsers = (await UserService.GetAll())
                               .Where(user => feature.Users.Contains(user))
                               .ToList();
            var responsibleUserId = task.ResponsibleUser != null ? task.ResponsibleUser.Id : 0;
            return new EditTaskViewModel
            {
                TaskId = task.ID,
                FeatureId = feature.ID,
                Name = task.Name,
                Description = task.Description,
                DeadLine = task.DeadLine,
                State = (int)task.State,
                UserId = responsibleUserId,
                PosibleUsers = posibleUsers
            };
        }

        #endregion

        #region DeteleTask

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> DeleteTask(int id, int taskId)
        {
            var task = await TaskService.GetById(taskId);
            if (task == null)
                return BadRequest("Задача не найдена.");

            var feature = await FeatureService.GetById(id);
            if (!feature.Tasks.Contains(task))
                return BadRequest("Фича/Гипотеза не содержит эту задачу.");

            feature.Tasks.Remove(task);
            await FeatureService.Update(feature);
            await TaskService.Delete(task.ID);
            return RedirectToAction("Index");
        }

        #endregion
    }
}

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
    public class TaskController : BaseController
    {
        public TaskController(IServiceProvider serviceProvider):base(serviceProvider)
        {
            
        }
        public async Task<IActionResult> Index([FromQuery] bool error)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var tasks = await TaskService.GetTaskList();
            return View(new TaskViewModel
            {
                Error = error,
                ProductId = Product.ID,
                Tasks = tasks
            });
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(new NewTaskViewModel 
            {
                ProductId = Product.ID,
                PosibleUsers = await UserService.GetUsersList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Users == null)
                {
                    return RedirectToAction("Index", "Task", new { error = true });
                }
                var team = await UserService.GetUsersListByIds(model.Users);
                await TaskService.AddTask(model.Name, model.Description, team, model.DeadLine);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(TaskViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetEditTaskViewModel(model.TaskId));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(EditTaskViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetEditTaskViewModel(model.ID));

            var team = await UserService.GetUsersListByIds(model.Users);
            await TaskService.UpdateTask(new Entity.Task
            {
                ID = model.ID,
                Name = model.Name,
                Description = model.Description, 
                Team = team,
                DeadLine = model.DeadLine,
                State = model.State
            });

            return RedirectToAction("Index", "Task");
        }

        private async Task<EditTaskViewModel> GetEditTaskViewModel(int taskId)
        {
            var task = await TaskService.GetTaskById(taskId);
            return new EditTaskViewModel
            {
                ID = task.ID,
                Name = task.Name,
                Description = task.Description,
                DeadLine = task.DeadLine,
                State = task.State,
                Users = task.Team.Select(x => x.Id).ToList(),
                ProductId = 1, // todo take from DB
                PosibleUsers = await UserService.GetUsersList()
            };
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] int taskId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await TaskService.DeleteTask(taskId);
            return RedirectToAction("Index", "Task");
        }

        [HttpGet]
        public async Task<IActionResult> EditState(TaskViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var task = await TaskService.GetTaskById(model.TaskId);
            return View(new EditStateTaskViewModel
            {
                TaskId = task.ID,
                Task = task
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditState(EditStateTaskViewModel model)
        {
            // todo save state changes
            return RedirectToAction("Index", "Task");
        }
    }
}

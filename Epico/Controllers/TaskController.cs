using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    public class TaskController : BaseController
    {
        public TaskController(IServiceProvider serviceProvider):base(serviceProvider)
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            return View(new NewTaskViewModel 
            { 
                PosibleUsers = await UserService.GetUsersList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var team = await UserService.GetUsersListByIds(model.Users);
                await TaskService.AddTask(model.Name, model.Description, team, model.DeadLine);
            }
            return Ok("Задача создана");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int projectId, [FromQuery] int taskId)
        {
            // todo прикрутить вытаскивание из базы
            var task = new Entity.Task
            {
                ID = 123,
                Name = "задача 123 тест",
                Description = "описание задачи 123 тест",
                DeadLine = DateTime.Today,
                State = TaskState.NotStarted,
                Team = new List<User>()
            };

            return View(new EditTaskViewModel
            {
                ID = task.ID,
                Name = task.Name,
                Description = task.Description,
                DeadLine = task.DeadLine,
                State = task.State,
                Users = task.Team.Select(x => x.Id).ToList(),
                ProjectId = projectId,
                PosibleUsers = await UserService.GetUsersList()
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(EditTaskViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            // todo прикрутить вытаскивание юзеров из базы
            var team = new List<User>();
            await TaskService.UpdateTask(model.ID, model.Name, model.Description, team, model.DeadLine);
            return Ok("Задача создана");
        }

        public async Task<IActionResult> Delete([FromQuery] int taskId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            // todo Прикрутить удаление задачи из базы
            await TaskService.DeleteTask(taskId);
            return Ok("Задача удалена");
        }
    }
}

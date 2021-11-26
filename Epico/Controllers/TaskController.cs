﻿using Epico.Entity;
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
            return RedirectToAction("View", "Project", new { id = model.ProjectId });
            //return Ok("Задача создана");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int projectId, [FromQuery] int taskId)
        {
            var task = await TaskService.GetTaskById(taskId);

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
            
            return Ok("Задача изменена");
        }

        public async Task<IActionResult> Delete([FromQuery] int taskId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await TaskService.DeleteTask(taskId);
            return Ok("Задача удалена");
        }
    }
}

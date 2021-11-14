using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly TeamService _teamService;
        public TaskController(IServiceProvider serviceProvider)
        {
            _taskService = serviceProvider.GetService(typeof(TaskService)) as TaskService;
            _teamService = serviceProvider.GetService(typeof(TeamService)) as TeamService;
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
                PosibleTeams = new List<Team> // Для теста
                { new Team { Name = "фича1" }, new Team { Name = "фича2" } } // await _featureService.GetTeams()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест                
                var team = new Team(); // await _teamService.GetTeamById(model.TeamId);
                await _taskService.AddTask(model.Name, model.Description, team, model.DeadLine);
            }
            return Ok("Задача создана");
        }
    }
}

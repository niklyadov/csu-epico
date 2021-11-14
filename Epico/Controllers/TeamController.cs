using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epico.Controllers
{
    public class TeamController : Controller
    {
        private readonly TeamService _teamService;
        private readonly UserService _userService;
        public TeamController(IServiceProvider serviceProvider)
        {
            _teamService = serviceProvider.GetService(typeof(TeamService)) as TeamService;
            _userService = serviceProvider.GetService(typeof(UserService)) as UserService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            return View(new NewTeamViewModel
            {
                PosibleUsers = new List<Entity.User> { new Entity.User { UserName = "Юзер 1" }, new Entity.User { UserName = "Юзер 2" } },
                PosiblePositions = Enum.GetValues(typeof(TeamPosition)).Cast<TeamPosition>()
            });
        }

        [HttpPost]
        public IActionResult New(NewTeamViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Тест
                var users = new List<Entity.User> { new Entity.User() };  // _userService.GetUser(model.UsersId);
                _teamService.AddTeam(model.Name, model.Position, users);
            }
            return Ok("Команда добавлена");
        }
    }
}

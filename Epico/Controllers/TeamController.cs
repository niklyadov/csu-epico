using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{

    [Authorize]
    public class TeamController : BaseController
    {
        public TeamController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var users = await UserService.GetAll();
            return View(new TeamViewModel
            {
                Users = users
            });
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var user = await UserService.GetById(id);
            if (await AccountService.IsManager(user))
                return BadRequest("Нельзя удалить менеджера.");

            var tasks = (await TaskService.GetAll())
               .Where(task => task.ResponsibleUserId == id)
               .ToList();
            tasks.ForEach(task => task.ResponsibleUser = null);

            await TaskService.UpdateRange(tasks);
            await UserService.Delete(id);
            return RedirectToAction("Index", "Team");
        }

        #endregion
    }
}

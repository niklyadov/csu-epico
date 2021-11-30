using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Epico.Models
{
    public class AddUserToTeamViewModel
    {
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public List<User> PosibleUsers { get; set; }

        [BindProperty] public List<int> UserIds { get; set; }
    }
}

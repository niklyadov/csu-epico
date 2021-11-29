using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class AddUserToTeamViewModel
    {
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public List<User> PosibleUsers { get; set; }

        [BindProperty] public List<string> UserIds { get; set; }
    }
}

using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class AddTeamViewModel
    {
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public List<User> Users { get; set; }

        public string UserId { get; set; }
    }
}

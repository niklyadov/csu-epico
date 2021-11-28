using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class AddTeamViewModel
    {
        public string TeamName { get; set; }
        public int TeamId { get; set; }
        public List<User> Users { get; set; }

        public string UserId { get; set; }
    }
}

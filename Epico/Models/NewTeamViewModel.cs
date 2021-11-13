using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class NewTeamViewModel
    {
        public string Name { get; set; }
        public TeamPosition Position { get; set; }
        public List<User> Users { get; set; }
        public IEnumerable<TeamPosition> PosiblePositions { get; set; }
        public List<User> PosibleUsers { get; set; }
    }
}

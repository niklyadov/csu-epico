using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class TeamViewModel
    {
        public int TaskId { get; set; }
        public List<Entity.Task> Tasks { get; set; }
    }
}

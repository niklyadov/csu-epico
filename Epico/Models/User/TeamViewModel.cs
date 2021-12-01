using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class TeamViewModel
    {
        public int TaskId { get; set; }
        //public List<Entity.Task> Tasks { get; set; }
        public List<User> Users { get; set; }
    }
}

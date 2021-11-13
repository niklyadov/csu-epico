using System.Collections.Generic;

namespace Epico.Models
{
    public class NewSprintViewModel
    {
        public string Name { get; set; }
        public Entity.Task Tasks { get; set; } // List<Task> 
        public List<Entity.Task> PosibleTasks { get; set; } // List<Task> 
    }
}

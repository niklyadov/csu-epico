using System.Collections.Generic;

namespace Epico.Models
{
    public class TaskViewModel
    {
        public int ProductId { get; set; }
        public List<Entity.Task> Tasks { get; set; }
        public bool Error { get; set; }
        public int TaskId { get; set; }
    }
}

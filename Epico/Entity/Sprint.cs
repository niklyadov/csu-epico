using System.Collections.Generic;

namespace Epico.Entity
{
    public class Sprint
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
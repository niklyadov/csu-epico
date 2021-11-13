using Epico.Entity.DAL;
using System.Collections.Generic;

namespace Epico.Entity
{
    public class Sprint : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
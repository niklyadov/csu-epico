using System.Collections.Generic;
using Epico.Entity.DAL;

namespace Epico.Entity
{
    public class Sprint : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
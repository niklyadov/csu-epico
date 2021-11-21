using System.Collections.Generic;
using Epico.Entity.DAL;

namespace Epico.Entity
{
    public class Project : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string ProductFormula { get; set; }
        public string OwnerUserId { get; set; }

        public List<Sprint> Sprints { get; set; }
            = new List<Sprint>();


        /*
        public List<Task> Tasks { get; set; }
        public List<Metric> Metrics { get; set; }
        public List<User> Roadmaps { get; set; }
        */
    }
}
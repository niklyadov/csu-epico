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
        public List<Metric> Metrics { get; set; }
        public List<Sprint> Sprints { get; set; }
        public List<Roadmap> Roadmaps { get; set; }
    }
}
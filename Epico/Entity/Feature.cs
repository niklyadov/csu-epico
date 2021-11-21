using Epico.Entity.DAL;
using System.Collections.Generic;

namespace Epico.Entity
{
    public class Feature : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hypothesis { get; set; }
        public List<Metric> Metric { get; set; }
            = new List<Metric>();
        public List<Entity.Task> Tasks { get; set; } 
            = new List<Entity.Task>();
        public FeatureState State { get; set; }
        public RoadmapType Roadmap { get; set; }
    }
}
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
        public Metric Metric { get; set; }
        public FeatureState State { get; set; }
        public List<Entity.Task> Tasks { get; set; }
    }
}
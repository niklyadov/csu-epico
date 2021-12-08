using Epico.Entity.DAL;
using System.Collections.Generic;

namespace Epico.Entity
{
    public class Metric : IEntity
    {
        public int ID { get; set; }
        public int? ParentMetricId { get; set; }
        public bool IsNSM { get { return ParentMetricId == null; } }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Feature> Features { get; set; }
            = new List<Feature>();
        public List<Metric> Children { get; set; } 
            = new List<Metric>();
    }
}
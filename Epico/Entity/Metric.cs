using Epico.Entity.DAL;

namespace Epico.Entity
{
    public class Metric : IEntity
    {
        public int ID { get; set; }
        public int? ParentMetricId { get; set; }
        public bool IsNSM { get { return ParentMetricId == null; } }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
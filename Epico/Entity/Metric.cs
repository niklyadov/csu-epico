namespace Epico.Entity
{
    public class Metric
    {
        public int ID { get; set; }
        public Metric ParentMetric { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
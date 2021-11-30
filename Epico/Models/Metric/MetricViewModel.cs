using Epico.Entity;

namespace Epico.Models
{
    public class MetricViewModel
    {
        public int ProductId { get; set; }
        public Metric Metric { get; set; }
        public bool Error { get; set; }
    }
}

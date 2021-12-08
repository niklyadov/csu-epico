using Epico.Entity;

namespace Epico.Models
{
    public class MetricViewModel
    {
        public Metric Metric { get; set; }
        public bool Error { get; set; }
        public bool ParentError { get; set; }
        public bool DeleteMetricError { get; set; }
    }
}

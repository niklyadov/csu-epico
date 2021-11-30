using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class FeatureViewModel
    {
        public int FeatureId { get; set; }
        public List<Feature> Features { get; set; }
        public bool TaskError { get; set; }
        public bool MetricError { get; set; }
    }
}

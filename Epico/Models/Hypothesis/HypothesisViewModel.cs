using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class HypothesisViewModel
    {
        public int HypothesisId { get; set; }
        public List<Feature> Hypothesis { get; set; }
    }
}

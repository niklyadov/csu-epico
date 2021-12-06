using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class HypothesisViewModel
    {
        public int HypothesisId { get; set; }
        public List<Feature> Hypothesis { get; set; }
    }
}

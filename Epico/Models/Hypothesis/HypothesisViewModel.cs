using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class HypothesisViewModel
    {
        public int HypothisisId { get; set; }
        public List<Feature> Hypothisis { get; set; }
    }
}

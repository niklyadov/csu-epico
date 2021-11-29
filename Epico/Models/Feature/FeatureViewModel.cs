using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class FeatureViewModel
    {
        public int FeatureId { get; set; }
        public int ProductId { get; set; }
        public List<Feature> Features { get; set; }
    }
}

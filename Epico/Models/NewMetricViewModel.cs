using Epico.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class NewMetricViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentMetricId { get; set; }
        public List<Metric> AvailableParentMetrics { get; set; }
    }
}

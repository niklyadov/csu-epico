using Epico.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class EditMetricViewModel
    {
        public int MetricId { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Description { get; set; }
        public int? ParentMetricId { get; set; }

        public List<Metric> PosibleParentMetrics { get; set; }
    }
}

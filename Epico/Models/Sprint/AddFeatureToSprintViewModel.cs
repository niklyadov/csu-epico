using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Epico.Models
{
    public class AddFeatureToSprintViewModel
    {
        public int SprintId { get; set; }
        public string SprintName { get; set; }
        [BindProperty] public List<int> FeatureIds { get; set; }
        public List<Feature> PosibleFeatures { get; set; }
    }
}

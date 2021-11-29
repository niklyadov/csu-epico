using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

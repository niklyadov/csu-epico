using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Epico.Models
{
    public class EditSprintViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [BindProperty]
        public List<int> Features { get; set; }

        public int ProjectID { get; set; }
        public List<Feature> PosibleFeatures { get; set; }
    }
}

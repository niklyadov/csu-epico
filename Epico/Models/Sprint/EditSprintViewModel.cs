using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class EditSprintViewModel
    {
        public int SprintId { get; set; }
        [Required]
        public string Name { get; set; }
        [BindProperty]
        public List<int> Features { get; set; }

        public int ProductId { get; set; }
        public List<Feature> PosibleFeatures { get; set; }
    }
}

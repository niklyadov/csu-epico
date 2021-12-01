using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class NewSprintViewModel
    {
        [Required(ErrorMessage ="*Обязательное поле")]
        public string Name { get; set; }
        [BindProperty] 
        public List<int> Features { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }


        public List<Feature> PosibleFeatures { get; set; }
    }
}

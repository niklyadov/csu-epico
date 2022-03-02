using Epico.Entity;
using Epico.Models.ProductEntity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models.ProductEntity.Feature
{
    public class EditFeatureViewModel : EditProductEntityViewModel
    {
        [Required(ErrorMessage = "*Обязательно")]
        [BindProperty]
        public FeatureState State { get; set; }
    }
}

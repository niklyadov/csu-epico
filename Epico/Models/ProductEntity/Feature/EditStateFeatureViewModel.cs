using Epico.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Epico.Models.ProductEntity.Feature
{
    public class EditStateFeatureViewModel
    {
        public int Id { get; set; }
        public Entity.Feature Feature { get; set; }

        [BindProperty] 
        public FeatureState State { get; set; }
    }
}

using Epico.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Epico.Models
{
    public class EditStateFeatureViewModel
    {
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
        [BindProperty] public FeatureState State { get; set; }
    }
}

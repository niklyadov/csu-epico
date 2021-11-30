using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class NewProductViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Vision { get; set; }
        [Required]
        public string Mission { get; set; }
        [Required]
        public string ProductFormula { get; set; }
    }
}
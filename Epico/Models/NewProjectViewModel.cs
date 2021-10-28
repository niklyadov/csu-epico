using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class NewProjectViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string ProductFormula { get; set; }
    }
}
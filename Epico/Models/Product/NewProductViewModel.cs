using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class NewProductViewModel
    {
        [Required(ErrorMessage = "*������������ ����")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*������������ ����")]
        public string Vision { get; set; }
        [Required(ErrorMessage = "*������������ ����")]
        public string Mission { get; set; }
        [Required(ErrorMessage = "*������������ ����")]
        public string ProductFormula { get; set; }
    }
}
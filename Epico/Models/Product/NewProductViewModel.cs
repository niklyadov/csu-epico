using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class NewProductViewModel
    {
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Vision { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Mission { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public string ProductFormula { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Epico.Views
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*Обязательно поле")]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "*Обязательно поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
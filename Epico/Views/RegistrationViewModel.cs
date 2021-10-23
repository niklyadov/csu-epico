using System.ComponentModel.DataAnnotations;

namespace Epico.Views
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }
    }
}
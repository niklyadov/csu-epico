using System.ComponentModel.DataAnnotations;

namespace Epico.Views
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "*Обязательно поле")]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        [Required(ErrorMessage = "*Обязательно поле")]
        [Display(Name = "Должность пользователя")]
        public string Position { get; set; }

        [Required(ErrorMessage = "*Обязательно поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*Обязательно поле")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }
    }
}
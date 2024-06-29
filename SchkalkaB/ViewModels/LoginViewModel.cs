using System.ComponentModel.DataAnnotations;

namespace SchkalkaB.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Укажите имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Введите пароль")]
        public string Password { get; set; }
    }
}

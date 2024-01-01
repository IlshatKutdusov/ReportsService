using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "У пользователя не указан логин!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя логин должен содержать от 3 до 30 символов!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "У пользователя не указан пароль!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя пароль должен содержать от 3 до 30 символов!")]
        public string Password { get; set; }
    }
}

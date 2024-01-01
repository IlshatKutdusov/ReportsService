using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "У пользователя не указана фамилия!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя фамилия должна содержать от 3 до 30 символов!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "У пользователя не указано имя!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя имя должно содержать от 3 до 30 символов!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "У пользователя не указан Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "У пользователя не указан логин!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя логин должен содержать от 3 до 30 символов!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "У пользователя не указан пароль!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя пароль должен содержать от 3 до 30 символов!")]
        public string Password { get; set; }
    }
}

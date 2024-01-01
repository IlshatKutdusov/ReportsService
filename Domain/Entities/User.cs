using Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "У пользователя не указана фамилия!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя фамилия должна содержать от 3 до 30 символов!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "У пользователя не указано имя!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя имя должно содержать от 3 до 30 символов!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "У пользователя не указан логин!")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "У пользователя логин должен содержать от 3 до 30 символов!")]
        public string Login { get; set; }
        
        public IList<File> Files { get; set; }
    }
}

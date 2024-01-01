using Domain.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class File : BaseEntity
    {
        [Required(ErrorMessage = "У файла не указан id пользователя!")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "У файла не указано имя!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "У файла не указан путь!")]
        public string Path { get; set; }

        [Required(ErrorMessage = "У файла не указан размер!")]
        public int Size { get; set; }
        
        public IList<Report> Reports { get; set; }
    }
}

using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Report : BaseEntity
    {
        [Required(ErrorMessage = "У отчета не указан id пользователя!")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "У отчета не указан id файла!")]
        public int FileId { get; set; }

        [Required(ErrorMessage = "У отчета не указано имя!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "У отчета не указан путь!")]
        public string Path { get; set; }

        [Required(ErrorMessage = "У отчета не указан формат!")]
        public string Format { get; set; }

        [Required(ErrorMessage = "У отчета не указан поставщик!")]
        public string Provider { get; set; }
    }
}

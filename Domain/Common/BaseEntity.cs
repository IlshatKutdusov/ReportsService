using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; }

        public bool isActive { get; set; } = true;
    }
}

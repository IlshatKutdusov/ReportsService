using System;

namespace Domain.Common
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool isActive { get; set; }
    }
}

using Domain.Entities;

namespace Application.Common.Models.Responses
{
    public class UserResponse : DefaultResponse
    {
        public User? User { get; set; }
    }
}

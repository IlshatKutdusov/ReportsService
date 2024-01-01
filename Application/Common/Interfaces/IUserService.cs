using Application.Common.Models;
using Application.Common.Models.Responses;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> GetById(string requestUserLogin, int userId);
        Task<UserResponse> GetByLogin(string requestUserLogin, string userLogin);

        Task<DefaultResponse> Update(string requestUserLogin, User user);

        Task<DefaultResponse> Login(LoginModel loginModel);
        Task<DefaultResponse> Register(RegisterModel registerModel);
    }
}

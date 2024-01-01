using System.Threading.Tasks;
using Application.Common.Models.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<FileResponse> GetById(string requestUserLogin, int fileId);

        Task<FileStreamResponse> GetFile(string requestUserLogin, int fileId);

        Task<ProvidersResponse> GetProviders(string requestUserLogin, int fileId);

        Task<FileResponse> UploadFile(string requestUserLogin, string userLogin, IFormFile upload);

        Task<DefaultResponse> Update(string requestUserLogin, File file);

        Task<DefaultResponse> Remove(string requestUserLogin, int fileId);
    }
}

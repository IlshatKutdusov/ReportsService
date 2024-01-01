using Application.Common.Models.Responses;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IFileHelper
    {
        Task<DefaultResponse> SourceFileDataCheck(File file);

        Task<ProvidersResponse> GetProviders(File file);
    }
}

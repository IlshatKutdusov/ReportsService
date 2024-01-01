using Application.Common.Models.Responses;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IReportBuilder
    {
        Task<DefaultResponse> DefaultSaveAsExcel(User user, File file, Report report);

        Task<DefaultResponse> DefaultSaveAsPdf(User user, File file, Report report);

        Task<DefaultResponse> ProviderSaveAsExcel(string provider, User user, File file, Report report);

        Task<DefaultResponse> ProviderSaveAsPdf(string provider, User user, File file, Report report);
    }
}

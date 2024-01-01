using Application.Common.Models.Responses;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IReportService
    {
        Task<ReportResponse> GetById(string requestUserLogin, int reportId);

        Task<FileStreamResponse> GetFile(string requestUserLogin, int reportId);

        Task<ReportResponse> Generate(string requestUserLogin, int fileId, string format, string provider = "");

        Task<DefaultResponse> Update(string requestUserLogin, Report report);

        Task<DefaultResponse> Remove(string requestUserLogin, int reportId);
    }
}

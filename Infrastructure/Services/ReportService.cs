using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly string ApplicationPath = System.IO.Directory.GetCurrentDirectory() + "\\SourceData\\Reports\\";

        private readonly IMapper _mapper;
        private readonly IDatabaseService _databaseService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IReportBuilder _reportBuilder;

        public ReportService(IMapper mapper, IDatabaseService databaseService, IFileService fileService, IUserService userService, IReportBuilder reportBuilder)
        {
            _mapper = mapper;
            _databaseService = databaseService;
            _fileService = fileService;
            _userService = userService;
            _reportBuilder = reportBuilder;
        }

        public async Task<ReportResponse> GetById(string requestUserLogin, int reportId)
        {
            var report = await _databaseService.Get<Report>().FirstOrDefaultAsync(e => e.Id == reportId);

            if (report == null)
                return new ReportResponse()
                {
                    Status = "Error",
                    Message = "The report not found!",
                    Done = false
                };

            var userResponse = await _userService.GetById(requestUserLogin, report.UserId);

            if (!userResponse.Done)
                return new ReportResponse(userResponse);

            var fileResponse = await _fileService.GetById(requestUserLogin, report.FileId);

            if (!fileResponse.Done)
                return new ReportResponse(fileResponse);

            if (!System.IO.File.Exists(report.Path + report.Name))
            {
                await _databaseService.Remove<Report>(report);
                await _databaseService.SaveChangesAsync();

                return new ReportResponse()
                {
                    Status = "Error",
                    Message = "The report file not found!",
                    Done = false
                };
            }

            var entity = _mapper.Map<Report>(report);

            return new ReportResponse()
            {
                Status = "Success",
                Message = "The report found successfully!",
                Done = true,
                Report = entity
            };
        }

        public async Task<FileStreamResponse> GetFile(string requestUserLogin, int reportId)
        {
            var reportResponse = await GetById(requestUserLogin, reportId);

            if (!reportResponse.Done || reportResponse.Report == null)
                return new FileStreamResponse(reportResponse);

            var userResponse = await _userService.GetById(requestUserLogin, reportResponse.Report.UserId);

            if (!userResponse.Done)
                return new FileStreamResponse(userResponse);

            var fileResponse = await _fileService.GetById(requestUserLogin, reportResponse.Report.FileId);

            if (!fileResponse.Done)
                return new FileStreamResponse(fileResponse);

            var fs = new System.IO.FileStream(reportResponse.Report.Path + reportResponse.Report.Name, System.IO.FileMode.Open);

            return new FileStreamResponse()
            {
                Status = "Success",
                Message = "The report found successfully!",
                Done = true,
                FileStream = fs,
                FileFormat = "application/" + reportResponse.Report.Format.Substring(1),
                FileName = reportResponse.Report.Name
            };
        }

        public async Task<ReportResponse> Generate(string requestUserLogin, int fileId, string format, string provider = "")
        {
            var fileResponse = await _fileService.GetById(requestUserLogin, fileId);

            if (!fileResponse.Done || fileResponse.File == null)
                return new ReportResponse(fileResponse);

            var userResponse = await _userService.GetById(requestUserLogin, fileResponse.File.UserId);

            if (!(format == "xlsx" || format == "pdf"))
                return new ReportResponse()
                {
                    Status = "Error",
                    Message = "This format is not supported! (only .xlsx or .pdf)",
                    Done = false
                };

            var fileProvidersResponse = await _fileService.GetProviders(requestUserLogin, fileId);
            
            if (!fileProvidersResponse.Done || fileProvidersResponse.Providers == null)
                return new ReportResponse(fileProvidersResponse);

            if (provider == "")
                provider = "all";
            
            if (provider != "all" && !fileProvidersResponse.Providers.Contains(provider))
                return new ReportResponse()
                {
                    Status = "Error",
                    Message = "The provider not found!",
                    Done = false
                };

            var selectedFormat = ".xlsx";

            if (format == "pdf")
                selectedFormat = ".pdf";

            var innerMessage = "_";

            if (provider != "all")
                innerMessage += provider + "_";

            var newReport = new Report()
            {
                UserId = fileResponse.File.UserId,
                FileId = fileResponse.File.Id,
                Name = "Report" + innerMessage + fileResponse.File.Name.Remove(fileResponse.File.Name.Length - 4, 4) + selectedFormat,
                Path = ApplicationPath,
                Format = selectedFormat,
                Provider = provider
            };

            if (fileResponse.File.Reports != null)
                foreach (var report in fileResponse.File.Reports)
                    if (report.Name == newReport.Name)
                        return new ReportResponse()
                        {
                            Status = "Error",
                            Message = "A report of this type has already been created!",
                            Done = false
                        };

            var creationTask = provider == "all" 
                ? await Create(userResponse.User, fileResponse.File, newReport) 
                : await Create(userResponse.User, fileResponse.File, newReport, provider);

            if (creationTask.Done)
                return new ReportResponse(creationTask)
                {
                    Report = newReport
                };

            return new ReportResponse(creationTask);
        }

        public async Task<DefaultResponse> Update(string requestUserLogin, Report report)
        {
            var reportResponse = await GetById(requestUserLogin, report.Id);

            if (!reportResponse.Done)
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = reportResponse.Message,
                    Done = false
                };

            var entity = _mapper.Map<Report>(report);

            var updatingTask = _databaseService.Update(entity);
            updatingTask.Wait();

            await _databaseService.SaveChangesAsync();

            if (updatingTask.IsCompletedSuccessfully)
                return new DefaultResponse()
                {
                    Status = "Success",
                    Message = "The report updated successfully!",
                    Done = true
                };

            return new DefaultResponse()
            {
                Status = "Error",
                Message = "The report not updated!",
                Done = false
            };
        }

        public async Task<DefaultResponse> Remove(string requestUserLogin, int reportId)
        {
            var reportResponse = await GetById(requestUserLogin, reportId);

            if (!reportResponse.Done || reportResponse.Report == null)
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = reportResponse.Message,
                    Done = false
                };

            var removingTask = _databaseService.Remove(reportResponse.Report);
            removingTask.Wait();

            await _databaseService.SaveChangesAsync();

            if (!removingTask.IsCompletedSuccessfully)
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = "The report not removed!",
                    Done = false
                };

            if (System.IO.File.Exists(reportResponse.Report.Path + reportResponse.Report.Name))
                System.IO.File.Delete(reportResponse.Report.Path + reportResponse.Report.Name);

            return new DefaultResponse()
            {
                Status = "Success",
                Message = "The report removed successfully!",
                Done = true
            };
        }

        private async Task<DefaultResponse> Create(User user, File file, Report report)
        {
            var entity = _mapper.Map<Report>(report);

            var addingTask = _databaseService.Add(entity);
            addingTask.Wait();

            if (!addingTask.IsCompletedSuccessfully)
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = "The report not added!",
                    Done = false
                };

            var generateTask = await Save(user, file, entity);

            if (generateTask.Done)
            {
                await _databaseService.SaveChangesAsync();

                return new DefaultResponse()
                {
                    Status = "Success",
                    Message = "The report created successfully!",
                    Done = true
                };
            }

            await _databaseService.Remove(entity);
            await _databaseService.SaveChangesAsync();

            return new DefaultResponse()
            {
                Status = "Error",
                Message = generateTask.Message,
                Done = false
            };
        }

        private async Task<DefaultResponse> Create(User user, File file, Report report, string provider)
        {
            if (provider == null)
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = "The provider cannot be NULL!",
                    Done = false
                };

            var entity = _mapper.Map<Report>(report);

            var addingTask = _databaseService.Add(entity);
            addingTask.Wait();

            if (!addingTask.IsCompletedSuccessfully)
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = "The report not added!",
                    Done = false
                };

            var generateTask = await Save(user, file, entity, provider);

            if (generateTask.Done)
            {
                await _databaseService.SaveChangesAsync();

                return new DefaultResponse()
                {
                    Status = "Success",
                    Message = "The report created successfully!",
                    Done = true
                };
            }

            await _databaseService.Remove(entity);
            await _databaseService.SaveChangesAsync();

            return new DefaultResponse()
            {
                Status = "Error",
                Message = generateTask.Message,
                Done = false
            };
        }

        private async Task<DefaultResponse> Save(User user, File file, Report report)
        {
            if (!System.IO.Directory.Exists(ApplicationPath))
                System.IO.Directory.CreateDirectory(ApplicationPath);

            string fromFile = file.Path + file.Name;

            if (!System.IO.File.Exists(fromFile))
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = "The file not uploaded!",
                    Done = false
                };

            var savingTask = new DefaultResponse();

            if (report.Format == ".xlsx")
                savingTask = await _reportBuilder.DefaultSaveAsExcel(user, file, report);

            if (report.Format == ".pdf")
                savingTask = await _reportBuilder.DefaultSaveAsPdf(user, file, report);

            if (savingTask.Done)
                return new DefaultResponse()
                {
                    Status = "Success",
                    Message = "The report generated successfully! (" + report.Format + ")",
                    Done = true
                };

            return new DefaultResponse()
            {
                Status = "Error",
                Message = "The report not generated! " + savingTask.Message,
                Done = false
            };
        }

        private async Task<DefaultResponse> Save(User user, File file, Report report, string provider)
        {
            if (!System.IO.Directory.Exists(ApplicationPath))
                System.IO.Directory.CreateDirectory(ApplicationPath);

            string fromFile = file.Path + file.Name;

            if (!System.IO.File.Exists(fromFile))
                return new DefaultResponse()
                {
                    Status = "Error",
                    Message = "The file not uploaded!",
                    Done = false
                };

            var savingTask = new DefaultResponse();

            if (report.Format == ".xlsx")
                savingTask = await _reportBuilder.ProviderSaveAsExcel(provider, user, file, report);

            if (report.Format == ".pdf")
                savingTask = await _reportBuilder.ProviderSaveAsPdf(provider, user, file, report);

            if (savingTask.Done)
                return new DefaultResponse()
                {
                    Status = "Success",
                    Message = "The report generated successfully! (" + report.Format + ")",
                    Done = true
                };

            return savingTask;
        }
    }
}

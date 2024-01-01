using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reports.Controllers
{
    public class ReportController : ApiControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int reportId)
        {
            try
            {
                var getByIdResponse = await _reportService.GetById(GetCurrentUserName(), reportId);

                if (getByIdResponse.Done)
                    return Ok(getByIdResponse);

                return BadRequest(new DefaultResponse() { Status = getByIdResponse.Status, Message = getByIdResponse.Message, Done = getByIdResponse.Done });
            }
            catch (Exception ex)
            {
                return BadRequest(new DefaultResponse()
                {
                    Status = "Error",
                    Message = "Message:  " + ex.Message,
                    Done = false
                });
            }
        }

        [HttpGet]
        [Route("File")]
        public async Task<IActionResult> GetFile(int reportId)
        {
            try
            {
                var getFileResponse = await _reportService.GetFile(GetCurrentUserName(), reportId);

                if (getFileResponse.Done)
                    return File(getFileResponse.FileStream, getFileResponse.FileFormat, getFileResponse.FileName);

                return BadRequest(new DefaultResponse() { Status = getFileResponse.Status, Message = getFileResponse.Message, Done = getFileResponse.Done});
            }
            catch (Exception ex)
            {
                return BadRequest(new DefaultResponse()
                {
                    Status = "Error",
                    Message = "Message:  " + ex.Message,
                    Done = false
                });
            }
        }


        [HttpPost]
        [Route("Generate")]
        public async Task<IActionResult> Generate(int fileId, string format)
        {
            try
            {
                var generateResponse = await _reportService.Generate(GetCurrentUserName(), fileId, format);

                if (generateResponse.Done)
                    return Ok(generateResponse);

                return BadRequest(new DefaultResponse() { Status = generateResponse.Status, Message = generateResponse.Message, Done = generateResponse.Done });
            }
            catch (Exception ex)
            {
                return BadRequest(new DefaultResponse()
                {
                    Status = "Error",
                    Message = "Message:  " + ex.Message,
                    Done = false
                });
            }
        }

        [HttpPost]
        [Route("GenerateWithProvider")]
        public async Task<IActionResult> Generate(int fileId, string format, string provider)
        {
            try
            {
                var generateResponse = await _reportService.Generate(GetCurrentUserName(), fileId, format, provider);

                if (generateResponse.Done)
                    return Ok(generateResponse);

                return BadRequest(new DefaultResponse() { Status = generateResponse.Status, Message = generateResponse.Message, Done = generateResponse.Done });
            }
            catch (Exception ex)
            {
                return BadRequest(new DefaultResponse()
                {
                    Status = "Error",
                    Message = "Message:  " + ex.Message,
                    Done = false
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Report report)
        {
            try
            {
                var updateResponse = await _reportService.Update(GetCurrentUserName(), report);

                if (updateResponse.Done)
                    return Ok(updateResponse);

                return BadRequest(updateResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new DefaultResponse()
                {
                    Status = "Error",
                    Message = "Message:  " + ex.Message,
                    Done = false
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int reportId)
        {
            try
            {
                var deleteResponse = await _reportService.Remove(GetCurrentUserName(), reportId);

                if (deleteResponse.Done)
                    return Ok(deleteResponse);

                return BadRequest(deleteResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new DefaultResponse()
                {
                    Status = "Error",
                    Message = "Message:  " + ex.Message,
                    Done = false
                });
            }
        }

        private string GetCurrentUserName() => User.FindFirstValue(ClaimTypes.Name);
    }
}

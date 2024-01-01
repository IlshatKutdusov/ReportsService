using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reports.Controllers
{
    public class FileController : ApiControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int fileId)
        {
            try
            {
                var getByIdResponse = await _fileService.GetById(GetCurrentUserName(), fileId);

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
        public async Task<IActionResult> GetFile(int fileId)
        {
            try
            {
                var getFileResponse = await _fileService.GetFile(GetCurrentUserName(), fileId);

                if (getFileResponse.Done)
                    return File(getFileResponse.FileStream, getFileResponse.FileFormat, getFileResponse.FileName);

                return BadRequest(new DefaultResponse() { Status = getFileResponse.Status, Message = getFileResponse.Message, Done = getFileResponse.Done });
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
        [Route("Providers")]
        public async Task<IActionResult> GetProviders(int fileId)
        {
            try
            {
                var getProvidersResponse = await _fileService.GetProviders(GetCurrentUserName(), fileId);

                if (getProvidersResponse.Done)
                    return Ok(getProvidersResponse);

                return BadRequest(new DefaultResponse() { Status = getProvidersResponse.Status, Message = getProvidersResponse.Message, Done = getProvidersResponse.Done });
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
        [Route("Upload")]
        public async Task<IActionResult> Upload(string userLogin, IFormFile upload)
        {
            try
            {
                var uploadResponse = await _fileService.UploadFile(GetCurrentUserName(), userLogin, upload);

                if (uploadResponse.Done)
                    return Ok(uploadResponse);

                return BadRequest(new DefaultResponse() { Status = uploadResponse.Status, Message = uploadResponse.Message, Done = uploadResponse.Done });
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
        public async Task<IActionResult> Update(File file)
        {
            try
            {
                var updateResponse = await _fileService.Update(GetCurrentUserName(), file);

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
        public async Task<IActionResult> Delete(int fileId)
        {
            try
            {
                var deleteResponse = await _fileService.Remove(GetCurrentUserName(), fileId);

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

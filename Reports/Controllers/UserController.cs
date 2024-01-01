using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reports.Controllers
{
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var loginResponse = await _userService.Login(loginModel);

                if (loginResponse.Done)
                    return Ok(loginResponse);

                return BadRequest(loginResponse);
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


        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                var registerResponse = await _userService.Register(registerModel);

                if (registerResponse.Done)
                    return Ok(registerResponse);

                return BadRequest(registerResponse);
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
        public async Task<IActionResult> GetByLogin(string userLogin)
        {
            try
            {
                var getByLoginResponse = await _userService.GetByLogin(GetCurrentUserName(), userLogin);
                
                if (getByLoginResponse.Done)
                    return Ok(getByLoginResponse);

                return BadRequest(getByLoginResponse);
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
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                var updateResponse = await _userService.Update(GetCurrentUserName(), user);

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

        private string GetCurrentUserName() => User.FindFirstValue(ClaimTypes.Name);
    }
}

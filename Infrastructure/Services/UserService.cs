using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IDatabaseService _databaseService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IMapper mapper, IDatabaseService databaseService, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _databaseService = databaseService;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<UserResponse> GetById(string requestUserLogin, int userId)
        {
            var user = await _databaseService.Get<User>().FirstOrDefaultAsync(e => e.Login == requestUserLogin);

            if (user == null)
                return new UserResponse()
                {
                    Status = "Error",
                    Message = "User not found!",
                    Done = false
                };

            if (user.Id != userId)
                return new UserResponse()
                {
                    Status = "Error",
                    Message = "Access is denied!",
                    Done = false
                };

            user.Files = await _databaseService.Get<File>().Where(e => e.UserId == user.Id).ToListAsync();

            var reports = await _databaseService.Get<Report>().Where(e => e.UserId == user.Id).ToListAsync();

            foreach (var file in user.Files)
            {
                if (reports.Any(x => x.FileId == file.Id))
                {
                    file.Reports = reports.Where(x => x.FileId == file.Id).ToList();
                }
            }

            var entity = _mapper.Map<User>(user);

            return new UserResponse()
            {
                Status = "Success",
                Message = "User found successfully!",
                Done = true,
                User = entity
            };
        }

        public async Task<UserResponse> GetByLogin(string requestUserLogin, string userLogin)
        {
            var user = await _databaseService.Get<User>().FirstOrDefaultAsync(e => e.Login == requestUserLogin);

            if (user == null)
                return new UserResponse()
                {
                    Status = "Error",
                    Message = "User not found!",
                    Done = false
                };

            if (user.Login != userLogin)
                return new UserResponse()
                {
                    Status = "Error",
                    Message = "Access is denied!",
                    Done = false
                };

            
            user.Files = _databaseService.Get<File>().Where(e => e.UserId == user.Id).ToList();

            var reports = _databaseService.Get<Report>().Where(e => e.UserId == user.Id).ToList();

            foreach (var file in user.Files)
            {
                if (reports.Any(x => x.FileId == file.Id))
                {
                    file.Reports = reports.Where(x => x.FileId == file.Id).ToList();
                }
            }

            var entity = _mapper.Map<User>(user);

            return new UserResponse()
            {
                Status = "Success",
                Message = "User found successfully!",
                Done = true,
                User = entity
            };
        }

        private async Task<DefaultResponse> Create(User user)
        {
            var entity = _mapper.Map<User>(user);

            var creatingTask = _databaseService.Add(entity);
            creatingTask.Wait();

            await _databaseService.SaveChangesAsync();

            if (creatingTask.IsCompletedSuccessfully)
                return new DefaultResponse()
                {
                    Status = "Success",
                    Message = "User created successfully!",
                    Done = true
                }; 

            return new DefaultResponse()
            {
                Status = "Error",
                Message = "User not created!",
                Done = false
            };
        }

        public async Task<DefaultResponse> Update(string requestUserLogin, User user)
        {
            var requestUser = await _databaseService.Get<User>().FirstOrDefaultAsync(e => e.Login == requestUserLogin);

            if (requestUser == null)
                return new UserResponse()
                {
                    Status = "Error",
                    Message = "User not found!",
                    Done = false
                };

            if (requestUser.Login != user.Login)
                return new UserResponse()
                {
                    Status = "Error",
                    Message = "Access is denied!",
                    Done = false
                };

            var entity = _mapper.Map<User>(user);

            var updatingTask = _databaseService.Update(entity);
            updatingTask.Wait();

            await _databaseService.SaveChangesAsync();

            if (updatingTask.IsCompletedSuccessfully)
                return new DefaultResponse()
                {
                    Status = "Success",
                    Message = "User updated successfully!",
                    Done = true
                };

            return new DefaultResponse()
            {
                Status = "Error",
                Message = "User not updated!",
                Done = false
            };
        }

        public async Task<DefaultResponse> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Login);

            if (!(user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password)))
                return new DefaultResponse
                {
                    Status = "Error",
                    Message = "User not found!"
                };

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            //await _repos.SaveChangesAsync();

            return new DefaultResponse
            {
                Status = "Success",
                Message = "Token: " + new JwtSecurityTokenHandler().WriteToken(token),
                Done = true
            };
        }

        public async Task<DefaultResponse> Register(RegisterModel registerModel)
        {
            var userLoginExists = await _userManager.FindByNameAsync(registerModel.Login);

            if (userLoginExists != null)
                return new DefaultResponse { 
                    Status = "Error", 
                    Message = "User with this login already exists!",
                    Done = false
                };

            var user = new ApplicationUser()
            {
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.Login,
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (!result.Succeeded)
                return new DefaultResponse { 
                    Status = "Error", 
                    Message = "User creation failed! Errors: " + string.Join("; ", result.Errors.Select(x => x.Description)),
                    Done = false
                };

            var creationTask = await Create(new User()
            {
                Surname = registerModel.Surname,
                Name = registerModel.Name,
                Login = registerModel.Login
            });

            if (creationTask.Done)
                return new DefaultResponse
                {
                    Status = "Success",
                    Message = "User created successfully!",
                    Done = true
                };

            return new DefaultResponse
            {
                Status = "Error",
                Message = "User not created!",
                Done = false
            };
        }
    }
}

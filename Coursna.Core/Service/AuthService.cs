using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Coursna.Core.Contracts;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Coursna.Core.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        
        public AuthService(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<ApplicationUser> Register(UserRegisterDto request)
        {
            if (request.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new Exception("Admin accounts cannot be created via the public registration endpoint.");
            }

            if (await _repo.UserExists(request.Email))
                throw new Exception("User already exists");

            var userToCreate = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.Name,
                Role = request.Role ?? "Student", // Default to student
                IsApproved = request.Role == "Teacher" ? false : true, // Teachers need approval
                gradeId = request.GradeId
            };

            if (userToCreate.Role == "Student" && !string.IsNullOrEmpty(request.InviteCode))
            {
                var teacher = await _repo.GetByInviteCodeAsync(request.InviteCode);
                if (teacher != null)
                {
                    userToCreate.TeacherId = teacher.Id;
                }
                else
                {
                    throw new Exception("Invalid invite code");
                }
            }

            return await _repo.Register(userToCreate, request.Password);
        }

        public async Task<AuthResponseDto> Login(UserLoginDto request)
        {
            var user = await _repo.Login(request.Email, request.Password);
            if (user == null) 
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid email or password", Token = null }; // Unauthorized

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName ?? user.UserName),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var keyStr = _config["JWT:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto 
            {
                IsSuccess = true,
                Message = "Login successful",
                Token = tokenHandler.WriteToken(token),
                UserId = user.Id.ToString(),
                Email = user.Email,
                Name = user.FullName ?? user.UserName,
                Role = user.Role ?? "User"
            };
        }

        public async Task<UserResponseDto> GetMyTeacherAsync(string studentId)
        {
            var teacher = await _repo.GetTeacherForStudentAsync(studentId);
            if (teacher == null) return null;

            return new UserResponseDto
            {
                Id = teacher.Id,
                Email = teacher.Email,
                FullName = teacher.FullName,
                Role = teacher.Role
            };
        }

        public async Task<List<UserResponseDto>> GetMyStudentsAsync(string teacherId)
        {
            var students = await _repo.GetStudentsForTeacherAsync(teacherId);
            return students.Select(s => new UserResponseDto
            {
                Id = s.Id,
                Email = s.Email,
                FullName = s.FullName,
                Role = s.Role
            }).ToList();
        }
    }
}




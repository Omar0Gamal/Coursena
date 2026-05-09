using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> CreateJwtToken(ApplicationUser user)
        {
            // bageb el roles
            var roles = await _userManager.GetRolesAsync(user);
            //make the payload
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                 new Claim(ClaimTypes.Name, user.FullName)
            };
            // ba7ot el roles 
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // el key elly ana 3arfo k backend    
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Key"])
                );
            // el signature
            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
            // take from config 
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );
            // return el token fy 4akl string 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Coursna.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Infrastrcuter.Identity
{
    public class IdentitySeeder : IIdentitySeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentitySeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task SeedAdminAsync()
        {
            var email = _configuration["AdminSettings:Email"];
            var password = _configuration["AdminSettings:Password"];
            var fullName = _configuration["AdminSettings:FullName"];

            if (!new EmailAddressAttribute().IsValid(email))
                throw new Exception("Invalid email format");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FullName = fullName,
                    IsApproved = true
                };

                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Admin");

                if (!roleResult.Succeeded)
                {
                    throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        public async Task SeedRolesAsync()
        {
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles) {
                if(string.IsNullOrEmpty(role)) throw new Exception("Role can't be empty");
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result=await _roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role : {role}");
                    }
                }
            }
        }
    }
}

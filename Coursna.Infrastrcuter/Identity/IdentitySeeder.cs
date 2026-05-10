using Coursna.Core.Domain.Entities;
using Coursna.Core.Contracts;
using Coursna.Infrastrcuter.DataContext;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _context;
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _configuration;

        public IdentitySeeder(AppDbContext context, IAuthRepository authRepo, IConfiguration configuration)
        {
            _context = context;
            _authRepo = authRepo;
            _configuration = configuration;
        }

        public async Task SeedAdminAsync()
        {
            var email = _configuration["AdminSettings:Email"];
            var password = _configuration["AdminSettings:Password"];
            var fullName = _configuration["AdminSettings:FullName"];

            if (!new EmailAddressAttribute().IsValid(email))
                throw new Exception("Invalid email format");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FullName = fullName,
                    Role = "Admin",
                    IsApproved = true
                };

                await _authRepo.Register(user, password);
            }
        }

        public async Task SeedRolesAsync()
        {
            // IdentityRoles are removed as we dropped Identity.
            // If you have a UserType table in the database, this is where you'd seed them.
            // For now, replacing this with a no-op or seed custom entities.
        }
    }
}



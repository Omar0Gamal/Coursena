using Coursna.Core.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Coursna.Core.Contracts;
using Coursna.Infrastrcuter.DataContext;
using System.Linq;
using System.Collections.Generic;

namespace Coursna.Infrastrcuter.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context) { _context = context; }

        public async Task<ApplicationUser> Register(ApplicationUser user, string password)
        {
            using var hmac = new HMACSHA512();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<ApplicationUser> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null || user.PasswordHash == null || user.PasswordSalt == null)
                return null;

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            if (computedHash.Length != user.PasswordHash.Length)
                return null;

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return null;
            }

            return user;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser> GetByInviteCodeAsync(string inviteCode)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.InviteCode == inviteCode);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetPendingTeachersAsync()
        {
            return await _context.Users.Where(u => u.IsApproved == false && u.Role == "Teacher").ToListAsync();
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(ApplicationUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetTeacherForStudentAsync(string studentId)
        {
            var student = await _context.Users
                .Include(u => u.Teacher)
                .FirstOrDefaultAsync(u => u.Id == studentId);
            return student?.Teacher;
        }

        public async Task<List<ApplicationUser>> GetStudentsForTeacherAsync(string teacherId)
        {
            return await _context.Users
                .Where(u => u.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<(int TotalUsers, int TotalTeachers, int TotalCourses, int PendingTeachers, int PendingCourses)> GetStatsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalTeachers = await _context.Users.CountAsync(u => u.Role == "Teacher");
            var totalCourses = await _context.Courses.CountAsync();
            var pendingTeachers = await _context.Users.CountAsync(u => u.Role == "Teacher" && !u.IsApproved);
            var pendingCourses = await _context.Courses.CountAsync(c => !c.IsApproved);
            return (totalUsers, totalTeachers, totalCourses, pendingTeachers, pendingCourses);
        }

        public async Task<List<ApplicationUser>> GetStudentsEnrolledInCourse(int courseId, string teacherId)
        {
            return await _context.Enrollments
                .Where(e => e.CourseId == courseId && e.course.TeacherId == teacherId)
                .Select(e => e.student)
     .          ToListAsync();
        }
    }
}



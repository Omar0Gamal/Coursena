using Coursna.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coursna.Core.Contracts
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> Register(ApplicationUser user, string password);
        Task<ApplicationUser> Login(string email, string password);
        Task<bool> UserExists(string email);
        
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetByInviteCodeAsync(string inviteCode);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<List<ApplicationUser>> GetPendingTeachersAsync();
        Task UpdateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(ApplicationUser user);
        Task<ApplicationUser> GetTeacherForStudentAsync(string studentId);
        Task<List<ApplicationUser>> GetStudentsForTeacherAsync(string teacherId);
    }
}


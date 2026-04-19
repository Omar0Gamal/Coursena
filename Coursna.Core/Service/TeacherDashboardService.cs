using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class TeacherDashboardService:ITeacherDashboardService
    {
        private readonly ITeacherDashboardRepository _repo;

        public TeacherDashboardService(ITeacherDashboardRepository repo)
        {
            _repo = repo;
        }
        public async Task<TeacherDashboardDto> GetDashboardAsync(string teacherId)
        {
            var totalCourses = await _repo.GetTotalCoursesAsync(teacherId);
            var totalCodes = await _repo.GetTotalCodesAsync(teacherId);
            var usedCodes = await _repo.GetUsedCodesAsync(teacherId);
            var totalStudents = await _repo.GetTotalStudentsAsync(teacherId);
            var activeStudents = await _repo.GetActiveStudentsAsync(teacherId);

            
            return TeacherDashboardDto.ToResponse(
                totalCourses,
                totalStudents,
                totalCodes,
                usedCodes,
                activeStudents
            );
        }
    }
}

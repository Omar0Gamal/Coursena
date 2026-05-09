using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.RepositoryInterface
{
  
        public interface IcourseRepository
        {
            Task<List<Course>> GetTeacherCousres(string id);

            Task<List<Course>> GetPublicCoursesByTeacherAsync(string teacherId);

            Task<List<Course>> GetByGradeIdAsync(int gradeId);

            Task<Course?> GetByIdWithTeacherAsync(int id);

            Task<List<Course>> GetPendingCoursesAsync();

            Task<List<Course>> SearchAsync(
                string? teacherId,
                int? gradeId,
                bool isPublic,
                string? searchBy,
                string? searchString);
        }
    
}

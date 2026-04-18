
using Coursna.Core.Domain.Entities;
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
    public class CourseContentService : ICourseContentService
    {
        private readonly ICourseContentRepository _courseContentRepo;
        private readonly IRepository<Course> _CourseRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;
        public CourseContentService(ICourseContentRepository courseContentRepo, IRepository<Course> courseRepo, IEnrollmentRepository enrollmentRepo)
        {
            _courseContentRepo = courseContentRepo;
            _CourseRepo = courseRepo;
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<AuthResponseDto> AddContentAsync(CreateContentDto dto, string teacherId)
        {
            var course = await _CourseRepo.GetByIdAsync(dto.CourseId);

            if (course == null)
                return AuthResponseDto.Fail("Course not found");

            if (course.TeacherId != teacherId)
                return AuthResponseDto.Fail("Unauthorized");

            var content = dto.ToEntity();

            await _courseContentRepo.AddAsync(content);
            await _courseContentRepo.SaveChangesAsync();

            return AuthResponseDto.Success("Content added successfully");
        }

        public async Task<List<CourseContentResponseDto>> GetCourseContentAsync(int courseId, string studentId)
        {
            var enrollment = await _enrollmentRepo
            .GetActiveEnrollmentAsync(studentId, courseId);

            if (enrollment == null)
                throw new Exception("Access denied");

            var contents = await _courseContentRepo.GetByCourseIdAsync(courseId);

            return contents
                .Select(c => c.ToCourseContentResponse
                ())
                .ToList();
        }
    }
}

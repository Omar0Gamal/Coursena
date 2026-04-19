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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly IRepository<Course> _courseRepo;
        private readonly ICourseCodeRepository _codeRepo;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepo,
            IRepository<Course> courseRepo,
            ICourseCodeRepository codeRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _courseRepo = courseRepo;
            _codeRepo = codeRepo;
        }

        public async Task<AuthResponseDto> EnrollByCodeAsync(string studentId, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return AuthResponseDto.Fail("Code is required");

            var courseCode = await _codeRepo.GetByCodeAsync(code);

            if (courseCode == null)
                return AuthResponseDto.Fail("Invalid code");

            if (courseCode.IsUsed)
                return AuthResponseDto.Fail("Code already used");

            var course = await _courseRepo.GetByIdAsync(courseCode.CourseId);

            if (course == null)
                return AuthResponseDto.Fail("Course not found");

           
            if (!course.IsApproved)
                return AuthResponseDto.Fail("Course not available");

            var existing = await _enrollmentRepo
                .GetActiveEnrollmentAsync(studentId, course.Id);

            if (existing != null)
                return AuthResponseDto.Fail("Already enrolled");

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = course.Id,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(course.DurationInDays)
            };

            await _enrollmentRepo.AddAsync(enrollment);
            await _enrollmentRepo.SaveChangesAsync();

            courseCode.IsUsed = true;
            courseCode.UsedAt = DateTime.UtcNow;
            courseCode.UsedByStudentId = studentId;

            await _codeRepo.UpdateAsync(courseCode);
            await _codeRepo.SaveChangesAsync();

            return AuthResponseDto.Success("Enrolled successfully");
        }

        public async Task<List<CourseResponseDto>> GetMyCoursesAsync(string studentId)
        {
            var courses = await _enrollmentRepo.GetStudentCoursesAsync(studentId);

            return courses
                .Select(c => c.ToResponse())
                .ToList();
        }
        public async Task<AuthResponseDto> CheckCompletionAsync(string studentId, int courseId)
        {
            var enrollment = await _enrollmentRepo.GetEnrollmentAsync(studentId, courseId);

            if (enrollment == null)
                return AuthResponseDto.Fail("Enrollment not found");

            
            if (DateTime.UtcNow >= enrollment.EndDate)
            {
                enrollment.IsCompleted = true;

                await _enrollmentRepo.UpdateAsync(enrollment);
                await _enrollmentRepo.SaveChangesAsync();

                return AuthResponseDto.Success("Course completed");
            }

            return AuthResponseDto.Fail("Course still active");
        }
    }
}

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
    public class CourseCodeService : ICourseCodeService
    {
        private readonly IRepository<Course> _courseRepo;
        private readonly ICourseCodeRepository _codeRepo;

        public CourseCodeService(IRepository<Course> courseRepo, ICourseCodeRepository codeRepo)
        {
            _courseRepo = courseRepo;
            _codeRepo = codeRepo;
        }

        public async Task<AuthResponseDto> GenerateCodesAsync(int courseId, int count)
        {
            if (count <= 0)
                return AuthResponseDto.Fail("Count must be greater than 0");

            var course = await _courseRepo.GetByIdAsync(courseId);

            if (course == null)
                return AuthResponseDto.Fail("Course not found");

            if (!course.IsApproved)
                return AuthResponseDto.Fail("Course must be approved first");

            var codes = new List<CourseCode>();

            for (int i = 0; i < count; i++)
            {
                codes.Add(new CourseCode
                {
                    Code = GenerateCode(),
                    CourseId = courseId
                });
            }

            await _codeRepo.AddRangeAsync(codes);

            return AuthResponseDto.Success($"{count} codes generated");
        }

        private string GenerateCode()
        {
            return Guid.NewGuid().ToString("N")[..8].ToUpper();
        }
        public async Task<List<CourseCodeResponseDto>> GetCodesAsync(string teacherId, int courseId)
        {
           
            var course = await _courseRepo.GetByIdAsync(courseId);

            
            if (course == null)
                return new List<CourseCodeResponseDto>();

         
            if (course.TeacherId != teacherId)
                return new List<CourseCodeResponseDto>();

            
            var codes = await _codeRepo.GetByCourseIdAsync(courseId);

           
            return codes.Select(c => new CourseCodeResponseDto
            {
                Code = c.Code,
                IsUsed = c.IsUsed,
                
            }).ToList();
        }
    }
}

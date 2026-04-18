using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _Repository;
        private readonly IcourseRepository _courseRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CourseService(IRepository<Course> repository,IcourseRepository courserepo,UserManager<ApplicationUser> userManager)
        {
            _Repository = repository;
            _courseRepository = courserepo;
            _userManager = userManager;

        }
        public async Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto, string teacherId)
        {
            var course = dto.ToEntity();
            course.TeacherId = teacherId;
            course.IsApproved = false;
            await _Repository.AddAsync(course);
            await _Repository.SaveChangesAsync();
            return course.ToResponse();

        }

        public async Task<string> GetInviteCodeAsync(string teacherId)
        {
            var teacher = await _userManager.FindByIdAsync(teacherId);

            if (teacher == null)
                return null;

            return teacher.InviteCode;
        }

        public async Task<CourseResponseDto?> GetByIdAsync(int id)
        {
           var course=await _Repository.GetByIdAsync(id);
            if (course == null)
            {
                throw new Exception("No course with this id");
            }
            return course.ToResponse();
        }

       

        public async Task<List<CourseResponseDto>> GetTeacherCoursesAsync(string teacherId)
        {
           var courses= await _courseRepository.GetTeacherCousres(teacherId);
            return courses.Select(c=>c.ToResponse()).ToList();
        }

        public async Task<bool> UpdateCourseAsync(int id, CreateCourseDto dto, string teacherId)
        {
            var course= await _Repository.GetByIdAsync(id);
            if(course == null)
                return false;
            if(course.TeacherId!=teacherId)
                return false;
            course.Title = dto.Title;
            course.Description = dto.Description;
             course.Price= dto.Price;
            course.DurationInDays = dto.DurationInDays;
            course.VideoUrl = dto.VideoUrl;
            course.content = dto.content;
            course.SubjectID = dto.SubjectId;
            course.GradeId = dto.GradeId;
            _Repository.UpdateAsync(course);
            await _Repository.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteCourseAsync(int id, string teacherId)
        {
            var course = await _Repository.GetByIdAsync(id);

            if (course == null)
                return false;

            if (course.TeacherId != teacherId)
                return false;

            _Repository.DeleteAsync(course);
            await _Repository.SaveChangesAsync();

            return true;
        }

        public async Task<List<CourseResponseDto>> GetPublicCoursesByInviteCodeAsync(string code)
        {
          
            if (string.IsNullOrWhiteSpace(code))
                return new List<CourseResponseDto>();

           
            var teacher = await _userManager.Users
                .FirstOrDefaultAsync(t => t.InviteCode == code);

            if (teacher == null)
                return new List<CourseResponseDto>();

           
            var courses = await _courseRepository
                .GetPublicCoursesByTeacherAsync(teacher.Id);

            
            return courses
                .Select(c => c.ToResponse())
                .ToList();
        }
        public async Task<List<CourseResponseDto>> GetAllCoursesAsync()
        {
            var courses = await _Repository.GetAllAsync();

            return courses
                .Select(c => c.ToResponse())
                .ToList();
        }

        public async Task<bool> ApproveCourseAsync(int id)
        {
            var course = await _Repository.GetByIdAsync(id);

            if (course == null)
                return false;

            course.IsApproved = true;

            _Repository.UpdateAsync(course);
            await _Repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectCourseAsync(int id)
        {
            var course = await _Repository.GetByIdAsync(id);

            if (course == null)
                return false;

            course.IsApproved = false;

            _Repository.UpdateAsync(course);
            await _Repository.SaveChangesAsync();

            return true;
        }
    }
}

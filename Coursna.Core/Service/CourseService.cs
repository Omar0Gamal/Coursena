using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Coursna.Core.Exceptions;
using Coursna.Core.Contracts;

namespace Coursna.Core.Service
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _Repository;
        private readonly IcourseRepository _courseRepository;
        private readonly IAuthRepository _authRepository;
        public CourseService(IRepository<Course> repository, IcourseRepository courserepo, IAuthRepository authRepository)
        {
            _Repository = repository;
            _courseRepository = courserepo;
            _authRepository = authRepository;

        }
        public async Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto, string teacherId)
        {
            var course = dto.ToEntity();
            course.TeacherId = teacherId;
            course.IsApproved = false;
            await _Repository.AddAsync(course);
            await _Repository.SaveChangesAsync();
            var createdCourse = await _courseRepository.GetByIdWithTeacherAsync(course.Id);
            return createdCourse.ToResponse();

        }

        public async Task<string> GetInviteCodeAsync(string teacherId)
        {
            var teacher = await _authRepository.GetUserByIdAsync(teacherId);

            if (teacher == null)
                throw new NotFoundException("Teacher not found");

            return teacher.InviteCode;
        }

        public async Task<CourseResponseDto?> GetByIdAsync(int id)
        {
            var course = await _Repository.GetByIdAsync(id);
            if (course == null)
            {
                throw new NotFoundException("No course with this id");
            }
            return course.ToResponse();
        }



        public async Task<List<CourseResponseDto>> GetTeacherCoursesAsync(string teacherId)
        {
            var courses = await _courseRepository.GetTeacherCousres(teacherId);
            return courses.Select(c => c.ToResponse()).ToList();
        }

        public async Task<bool> UpdateCourseAsync(int id, CreateCourseDto dto, string teacherId)
        {
            var course = await _Repository.GetByIdAsync(id);
            if (course == null)
                throw new NotFoundException("No course with this id");
            if (course.TeacherId != teacherId)
                throw new UnauthorizedAccessException("You are not the owner of this course");
            course.Title = dto.Title;
            course.Description = dto.Description;
            course.Price = dto.Price;
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
                throw new NotFoundException("No course with this id");

            if (course.TeacherId != teacherId)
                throw new UnauthorizedAccessException("You are not the owner of this course");

            _Repository.DeleteAsync(course);
            await _Repository.SaveChangesAsync();

            return true;
        }

        public async Task<List<CourseResponseDto>> GetPublicCoursesByInviteCodeAsync(string code)
        {

            if (string.IsNullOrWhiteSpace(code))
                throw new BadRequestException("Invite code is required");


            var users = await _authRepository.GetAllUsersAsync();
            var teacher = users.FirstOrDefault(t => t.InviteCode == code);

            if (teacher == null)
                throw new NotFoundException("Teacher not found for this invite code");


            var courses = await _courseRepository
                .GetPublicCoursesByTeacherAsync(teacher.Id);


            return courses
                .Select(c => c.ToResponse())
                .ToList();
        }
        public async Task<List<CourseResponseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetPendingCoursesAsync();

            return courses.Select(c => c.ToResponse()).ToList();
        }

        public async Task<bool> ApproveCourseAsync(int id)
        {
            var course = await _Repository.GetByIdAsync(id);

            if (course == null)
                throw new NotFoundException("no course with this id");

            course.IsApproved = true;

            _Repository.UpdateAsync(course);
            await _Repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectCourseAsync(int id)
        {
            var course = await _Repository.GetByIdAsync(id);

            if (course == null)
                throw new NotFoundException("no course with this id");

            course.IsApproved = false;

            _Repository.UpdateAsync(course);
            await _Repository.SaveChangesAsync();

            return true;
        }

        public async Task<List<CourseResponseDto>> GetCoursesForStudentAsync(string studentId)
        {
            var student = await _authRepository.GetUserByIdAsync(studentId);

            if (student == null)
                throw new NotFoundException("Student not found");

            if (student.gradeId == null)
                throw new BadRequestException("Student has no grade assigned");

            var courses = await _courseRepository
                .GetByGradeIdAsync(student.gradeId.Value);

            return courses
                .Select(c => c.ToResponse())
                .ToList();
        }


        ////////////////////// search//////////////////////
        public async Task<List<CourseResponseDto>> SearchPublicByTeacherAsync(
          string inviteCode,
          string searchBy,
          string searchString)
        {
            if (string.IsNullOrWhiteSpace(inviteCode))
                throw new BadRequestException("Invite code is required");


            var users = await _authRepository.GetAllUsersAsync();
            var teacher = users.FirstOrDefault(t => t.InviteCode == inviteCode);

            if (teacher == null)
                throw new NotFoundException("Invalid invite code");

            var courses = await _courseRepository.SearchAsync(
                teacherId: teacher.Id,
                gradeId: null,
                isPublic: false, //ll teacher el mo3yan
                searchBy: searchBy,
                searchString: searchString
            );

            return courses.Select(c => c.ToResponse()).ToList();
        }
        public async Task<List<CourseResponseDto>> SearchTeacherCoursesAsync(
    string teacherId,
    string searchBy,
    string searchString)
        {
            var courses = await _courseRepository.SearchAsync(
                teacherId: teacherId,
                gradeId: null,
                isPublic: false,
                searchBy: searchBy,
                searchString: searchString
            );

            return courses.Select(c => c.ToResponse()).ToList();
        }


        public async Task<List<CourseResponseDto>> SearchStudentCoursesAsync(
    string studentId,
    string searchBy,
    string searchString)
        {
            var student = await _authRepository.GetUserByIdAsync(studentId);

            if (student == null)
                throw new NotFoundException("Student not found");

            if (student.gradeId == null)
                throw new BadRequestException("Student has no grade");

            var courses = await _courseRepository.SearchAsync(
                teacherId: null,
                gradeId: student.gradeId,
                isPublic: false,
                searchBy: searchBy,
                searchString: searchString
            );

            return courses.Select(c => c.ToResponse()).ToList();
        }
    }
}
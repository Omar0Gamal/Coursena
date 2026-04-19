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
    public class ReviewService : IReviewService
    {
        private readonly IRepository<CourseReview> _reviewRepo;
        private readonly IEnrollmentRepository _enrollmentRepo;

        public ReviewService(
            IRepository<CourseReview> reviewRepo,
            IEnrollmentRepository enrollmentRepo)
        {
            _reviewRepo = reviewRepo;
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task<AuthResponseDto> AddReviewAsync(string studentId, CreateReviewDto dto)
        {
            
            var enrollment = await _enrollmentRepo
                .GetEnrollmentAsync(studentId, dto.CourseId);

            if (enrollment == null)
                return AuthResponseDto.Fail("You must enroll first");

            var review = new CourseReview
            {
                CourseId = dto.CourseId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                StudentId = studentId,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync();

            return AuthResponseDto.Success("Review added");
        }

        public async Task<List<ReviewResponseDto>> GetCourseReviewsAsync(int courseId)
        {
            var reviews = await _reviewRepo.GetAllAsync();

            return reviews
                .Where(r => r.CourseId == courseId)
                .Select(r=>r.ToResponse()).ToList();
        }
    }
}

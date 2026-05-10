using Coursna.Core.Domain.Entities;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddReview(CreateReviewDto dto)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reviewService.AddReviewAsync(studentId, dto);


            return Ok(result);
        }

        [HttpGet("/api/v1/courses/{courseId}/reviews")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviews(int courseId)
        {
            var result = await _reviewService.GetCourseReviewsAsync(courseId);
            return Ok(result);
        }
    }
}



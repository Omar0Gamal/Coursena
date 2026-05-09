using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(IReviewService reviewService,UserManager<ApplicationUser> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
        }

        [HttpPost("Add-Review")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddReview(CreateReviewDto dto)
        {
            var studentId = _userManager.GetUserId(User);

            var result = await _reviewService.AddReviewAsync(studentId, dto);


            return Ok(result);
        }

        [HttpGet("See-Reviews{courseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviews(int courseId)
        {
            var result = await _reviewService.GetCourseReviewsAsync(courseId);
            return Ok(result);
        }
    }
}

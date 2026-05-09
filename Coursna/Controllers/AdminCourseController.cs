using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/admin/courses")]
    [Authorize(Roles = "Admin")]
    public class AdminCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public AdminCourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseService.GetAllCoursesAsync();
            return Ok(result);
        }

       
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _courseService.ApproveCourseAsync(id);


            return Ok("Course approved successfully");
        }

    
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _courseService.RejectCourseAsync(id);


            return Ok("Course rejected successfully");
        }
    }
}
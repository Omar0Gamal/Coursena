using Coursna.Core.Domain.Entities;
using Coursna.Core.Dtos;
using Coursna.Core.Exceptions;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/student")]
    [Authorize(Roles = "Student")]
    public class StudentQuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IAttemptService _attemptService;

        public StudentQuizController(
            IQuizService quizService,
            IAttemptService attemptService)
        {
            _quizService = quizService;
            _attemptService = attemptService;
        }

        #region Quiz Listing

        // GET: /api/v1/student/courses/{courseId}/quizzes
        [HttpGet("courses/{courseId}/quizzes")]
        public async Task<IActionResult> GetPublishedQuizzes(int courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _quizService.GetPublishedQuizzesByCourseIdAsyc(courseId, studentId);
            return Ok(result);
        }

        // GET: /api/v1/student/quizzes/{quizId}/active-attempt
        [HttpGet("quizzes/{quizId}/active-attempt")]
        public async Task<IActionResult> GetActiveAttempt(int quizId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _attemptService.GetActiveAttemptAsync(quizId, studentId);
            return Ok(result);
        }

        // GET: /api/v1/student/quizzes/{courseId}/results
        [HttpGet("quizzes/{courseId}/results")]
        public async Task<IActionResult> GetCourseResults(int courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var result = await _attemptService.GetStudentAttemptsByCourseIdAsync(studentId, courseId);
                return Ok(result);
            }
            catch (NotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        #endregion

        #region Quiz Attempt Management

        // POST: /api/v1/student/quizzes/{quizId}/attempts
        [HttpPost("quizzes/{quizId}/attempts")]
        public async Task<IActionResult> StartAttempt(int quizId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var attemptId = await _attemptService.StartAttemptAsync(quizId,studentId);
                // Returns the location of the new resource
                return Created($"/api/v1/student/attempts/{attemptId}", new { AttemptId = attemptId });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }
        // GET: /api/v1/student/attempts/{attemptId}/questions
        [HttpGet("attempts/{attemptId}/questions")]
        public async Task<IActionResult> GetAttemptQuestions(int attemptId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                // This returns the quiz questions and options specifically for this attempt
                var questions = await _attemptService.GetAttemptQuestionsAsync(attemptId, studentId);
                return Ok(questions);
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { message = ex.Message }); }
        }
        // PATCH: /api/v1/student/attempts/{attemptId}/responses
        // Used for the "Heartbeat" auto-save feature in React
        [HttpPatch("attempts/{attemptId}/responses")]
        public async Task<IActionResult> SaveResponse(int attemptId, [FromBody] SaveResponseRequest request)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _attemptService.SaveResponseAsync(attemptId, request, studentId);
                return NoContent(); // 204 No Content is standard for background saves
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (UnauthorizedAccessException ex) { return StatusCode(403, new { message = ex.Message }); }
            catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        }

        // POST: /api/v1/student/attempts/{attemptId}/submit
        [HttpPost("attempts/{attemptId}/submit")]
        public async Task<IActionResult> SubmitAttempt(int attemptId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var result = await _attemptService.SubmitAttemptAsync(attemptId, studentId);
                return Ok(result); // Returns final score/result
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }
        // GET: /api/v1/student/attempts/{attemptId}/result
        [HttpGet("attempts/{attemptId}/result")]
        public async Task<IActionResult> GetAttemptResult(int attemptId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
             
                var result = await _attemptService.GetAttemptResultAsync(attemptId, studentId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        #endregion
    }
}


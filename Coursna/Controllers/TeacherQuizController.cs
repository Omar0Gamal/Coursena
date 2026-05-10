using Coursna.Core.Domain.Entities;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/v1/teacher")]
    [Authorize(Roles = "Teacher")]
    public class TeacherQuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;

        public TeacherQuizController(
            IQuizService quizService,
            IQuestionService questionService)
        {
            _quizService = quizService;
            _questionService = questionService;
        }

        #region Quiz Management

        // GET: /api/v1/teacher/courses/{courseId}/quizzes
        [HttpGet("courses/{courseId}/quizzes")]
        public async Task<IActionResult> GetQuizzes(int courseId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _quizService.GetQuizzesByCourseIdAsync(courseId, teacherId);
            return Ok(result);
        }

        // POST: /api/v1/teacher/courses/{courseId}/quizzes
        [HttpPost("courses/{courseId}/quizzes")]
        public async Task<IActionResult> CreateQuiz(int courseId, [FromBody] CreateQuizDto dto)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _quizService.CreateQuizAsync(dto, teacherId);
            return Created($"/api/v1/teacher/quizzes/{result.Id}", result);
        }

        // GET: /api/v1/teacher/quizzes/{quizId}
        [HttpGet("quizzes/{quizId}")]
        public async Task<IActionResult> GetQuiz(int quizId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var quiz = await _quizService.GetTeacherQuizWithQuestionsByIdAsync(quizId, teacherId);
                return Ok(quiz);
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        // PUT: /api/v1/teacher/quizzes/{quizId}
        [HttpPut("quizzes/{quizId}")]
        public async Task<IActionResult> UpdateQuiz(int quizId, [FromBody] CreateQuizDto dto)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _quizService.UpdateQuizAsync(quizId, dto, teacherId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
        }

        // DELETE: /api/v1/teacher/quizzes/{quizId}
        [HttpDelete("quizzes/{quizId}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _quizService.DeleteQuizAsync(quizId, teacherId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        // PATCH: /api/v1/teacher/quizzes/{quizId}/publish
        [HttpPatch("quizzes/{quizId}/publish")]
        public async Task<IActionResult> PublishQuiz(int quizId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _quizService.PublishQuizAsync(quizId, teacherId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
        }

        // GET: /api/v1/teacher/quizzes/{quizId}/answers
        [HttpGet("quizzes/{quizId}/answers")]
        public async Task<IActionResult> GetQuizAnswers(int quizId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var quiz = await _quizService.GetTeacherQuizWithAnswersByIdAsync(quizId, teacherId);
                return Ok(quiz);
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        }

        #endregion

        #region Question Management

        // POST: /api/v1/teacher/quizzes/{quizId}/questions
        [HttpPost("quizzes/{quizId}/questions")]
        public async Task<IActionResult> AddQuestion(int quizId, [FromBody] CreateQuestionDto request)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var questionId = await _quizService.AddQuestionAsync(quizId, request, teacherId);
                return Created($"/api/v1/teacher/questions/{questionId}", new { QuestionId = questionId });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
        }

        // PUT: /api/v1/teacher/questions/{questionId}
        [HttpPut("questions/{questionId}")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] CreateQuestionDto request)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _questionService.UpdateQuestionAsync(questionId, request, teacherId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
            catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        }

        // DELETE: /api/v1/teacher/questions/{questionId}
        [HttpDelete("questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _questionService.DeleteQuestionAsync(questionId, teacherId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
        }

        #endregion

        #region Option Management

        // POST: /api/v1/teacher/questions/{questionId}/options
        [HttpPost("questions/{questionId}/options")]
        public async Task<IActionResult> AddOption(int questionId, [FromBody] CreateOptionDto request)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var optionId = await _questionService.AddOptionAsync(questionId, request, teacherId);
                return Created($"/api/v1/teacher/options/{optionId}", new { OptionId = optionId });
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
        }

        // PUT: /api/v1/teacher/options/{optionId}
        [HttpPut("options/{optionId}")]
        public async Task<IActionResult> UpdateOption(int optionId, [FromBody] CreateOptionDto request)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _questionService.UpdateOptionAsync(optionId, request, teacherId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
        }

        // DELETE: /api/v1/teacher/options/{optionId}
        [HttpDelete("options/{optionId}")]
        public async Task<IActionResult> DeleteOption(int optionId)
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _questionService.DeleteOptionAsync(optionId, teacherId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { message = ex.Message }); }
        }

        #endregion
    }
}


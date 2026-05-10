using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class AttemptDetailsResponse
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal CurrentScore { get; set; }
        public AttemptStatus Status { get; set; }
        public QuizAttemptDto QuizAttemptDto { get; set; }
        public List<AttemptResponsesDto> Responses { get; set; } = new List<AttemptResponsesDto>();

    }
    public class QuizAttemptDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TimeLimitInMinutes { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
    public class AttemptResponsesDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<OptionResponseDto> Options { get; set; } = new List<OptionResponseDto>();
    }
    public class OptionResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }




}


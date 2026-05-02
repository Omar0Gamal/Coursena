using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class QuizWithAnswersDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public bool IsPublished { get; set; }
        public int MaxAttempts { get; set; }
        public int TimeLimitInMinutes { get; set; }
        public List<Questions> Questions { get; set; }
    }

    public class Questions
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<OptionWithAnswer> Options { get; set; }
    }
    public class OptionWithAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}


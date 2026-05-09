using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class QuizWithQuestionsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public bool IsPublished { get; set; }
        public int MaxAttempts { get; set; }
        public int TimeLimitInMinutes { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

        public class QuestionDto
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public List<OptionDto> Options { get; set; }
    }
    public class OptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}

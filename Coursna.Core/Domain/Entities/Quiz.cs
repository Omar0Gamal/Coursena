using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.Entities
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public Course course { get; set; }
        public int Description { get; set; }
        public bool IsPublished { get; set; }
        public int MaxAttempts { get; set; }
        public int TimeLimitInMinutes { get; set; }
        public List<Question>? Questions { get; set; }
        public ICollection<QuizAttempt> Attempts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.Entities
{
    public class StudentResponse
    {
        public int Id { get; set; }
        //[ForeignKey("QuizAttempt")]
        //public int QuizAttemptId { get; set; }
        //public QuizAttempt QuizAttempt { get; set; } = null!;
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int OptionId { get; set; }
        public string? TextValue { get; set; }
    }
}

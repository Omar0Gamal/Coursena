using Coursna.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.Entities
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string StudentId { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public decimal CurrentScore { get; set; }
        public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;
        public Quiz Quiz { get; set; } 
        public List<StudentResponse> Responses{ get; set; } = new List<StudentResponse>();
        
        public bool IsTimeExpired()
        {
            if (Quiz == null) throw new InvalidOperationException("Quiz data must be loaded to check time limits.");
            var expiresAt = StartedAt.AddMinutes(Quiz.TimeLimitInMinutes);
            return DateTime.UtcNow > expiresAt.AddSeconds(30);
        }


    }
}

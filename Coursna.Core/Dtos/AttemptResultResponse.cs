using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class AttemptResultResponse
    {
     public int AttemptId { get; set; }
     public decimal TotalScore { get; set; }
     public  DateTime CompletedAt { get; set; }
     public int QuizId { get; set; }
     public string QuizTitle { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class AttemptResultResponse
    {
        int AttemptId { get; set; }
        decimal TotalScore { get; set; }
        bool Passed { get; set; }
        DateTime CompletedAt { get; set; }
    }
}

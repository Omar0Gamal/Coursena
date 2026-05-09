using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class CreateQuizDto
    {
        public string Title { get; set; }
        public int CourseId { get; set; }
        public bool IsPublished { get; set; }=false;
        public int MaxAttempts { get; set; }
        public int TimeLimitInMinutes { get; set; }
    }
}

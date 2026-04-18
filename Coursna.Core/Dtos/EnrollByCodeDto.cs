using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class EnrollByCodeDto
    {
        public int CourseId { get; set; }
        public string Code { get; set; } = null!;
    }
}

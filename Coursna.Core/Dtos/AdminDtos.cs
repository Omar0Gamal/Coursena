using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class StateDto
    {
        public int TotalUsers { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalCourses { get; set; }
        public int PendingTeachers { get; set; }
        public int PendingCourses { get; set; }
    }
}

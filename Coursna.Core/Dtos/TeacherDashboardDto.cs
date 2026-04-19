using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class TeacherDashboardDto
    {
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public int TotalCodes { get; set; }
        public int UsedCodes { get; set; }
        public int ActiveStudents { get; set; }

  
        public static TeacherDashboardDto ToResponse(
            int totalCourses,
            int totalStudents,
            int totalCodes,
            int usedCodes,
            int activeStudents)
        {
            return new TeacherDashboardDto
            {
                TotalCourses = totalCourses,
                TotalStudents = totalStudents,
                TotalCodes = totalCodes,
                UsedCodes = usedCodes,
                ActiveStudents = activeStudents
            };
        }
    }
}

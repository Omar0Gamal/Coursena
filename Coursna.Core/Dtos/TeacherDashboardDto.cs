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
        public int ActiveCourses { get; set; }
        public int TotalStudents { get; set; }
        public int TotalCodes { get; set; }
        public int UsedCodes { get; set; }
        public int ActiveStudents { get; set; }
        public decimal MonthlyRevenue { get; set; }

  
        public static TeacherDashboardDto ToResponse(
            int totalCourses,
            int activeCourses,
            int totalStudents,
            int totalCodes,
            int usedCodes,
            int activeStudents,
            decimal monthlyRevenue)
        {
            return new TeacherDashboardDto
            {
                TotalCourses = totalCourses,
                ActiveCourses = activeCourses,
                TotalStudents = totalStudents,
                TotalCodes = totalCodes,
                UsedCodes = usedCodes,
                ActiveStudents = activeStudents,
                MonthlyRevenue = monthlyRevenue
            };
        }
    }
}

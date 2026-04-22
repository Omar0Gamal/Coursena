using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class CourseResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsApproved { get; set; }
        public string TeacherName { get; set; }
    }
        public static class CourseExtentsion
        {
            public static CourseResponseDto ToResponse(this Course course)
            {
                return new CourseResponseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    Price = course.Price,
                    IsApproved = course.IsApproved,
                    TeacherName = course.Teacher.FullName
                };
            }
        }
    
}

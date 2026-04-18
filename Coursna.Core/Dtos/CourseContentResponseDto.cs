using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class CourseContentResponseDto
    {

        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string DocumentUrl { get; set; }
        public string AssignmentUrl { get; set; }
        public int Order { get; set; }

        
    }
    public static class CourseContentExtenstion
    {
        public static CourseContentResponseDto ToCourseContentResponse(this CourseContent courseContent)
        {
            return new CourseContentResponseDto
            {
                Title = courseContent.Title,
                VideoUrl = courseContent.VideoUrl,
                DocumentUrl = courseContent.DocumentUrl,
                AssignmentUrl = courseContent.AssignmentUrl,
                Order = courseContent.order
            };
        }
    }
}

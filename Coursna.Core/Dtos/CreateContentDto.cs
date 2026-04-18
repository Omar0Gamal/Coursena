using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class CreateContentDto
    {
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string DocumentUrl { get; set; }
        public string AssignmentUrl { get; set; }
        public int order { get; set; }
        public int CourseId { get; set; }

        public CourseContent ToEntity() {
            return new CourseContent
            {
                Title = Title,
                VideoUrl = VideoUrl,
                DocumentUrl = DocumentUrl,
                AssignmentUrl = AssignmentUrl,
                order = order,
                CourseId = CourseId
            };
        }
    }
}

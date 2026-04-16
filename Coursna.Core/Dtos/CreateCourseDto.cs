using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class CreateCourseDto
    {
        public string Title {  get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public string VideoUrl { get; set; }
        public string content { get; set; }
        public int SubjectId {  get; set; }
        public int GradeId {  get; set; }

        public Course ToEntity()
        {
            return new Course
            {
                Title = Title,
                Description = Description,
                Price = Price,
                DurationInDays = DurationInDays,
                VideoUrl = VideoUrl,
                content = content,
                SubjectID = SubjectId,
                GradeId = GradeId,
                IsApproved = false
            };
        }
    }
}

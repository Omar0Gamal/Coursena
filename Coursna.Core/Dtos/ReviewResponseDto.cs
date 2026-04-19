using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class ReviewResponseDto
    {
        public string StudentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public static class ReviewExtenstion
    {
        public static ReviewResponseDto ToResponse(this CourseReview response)
        {
            return new ReviewResponseDto { StudentId = response.StudentId, Rating = response.Rating, Comment = response.Comment, CreatedAt = response.CreatedAt };
        }
    }
}

using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class CreateReviewDto
    {
        public int CourseId {  get; set; }
        public int Rating { get; set; }
        public string? Comment {  get; set; }


        
    }
}

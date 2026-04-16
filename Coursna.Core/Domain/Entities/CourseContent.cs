using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.Entities
{
    public class CourseContent
    {
        public int Id {  get; set; }
        public string Title {  get; set; }
        public string VideoUrl {  get; set; }
        public string DocumentUrl {  get; set; }
        public string AssignmentUrl {  get; set; }
        public int order { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}

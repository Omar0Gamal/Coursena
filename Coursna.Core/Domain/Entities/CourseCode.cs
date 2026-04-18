using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.Entities
{
    public class CourseCode
    {
        public int Id { get; set; }
        public string Code {  get; set; }
        public int CourseId {  get; set; }
        public Course Course { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; }
        public string? UsedByStudentId { get; set; }
    }
}

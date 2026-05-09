using Coursna.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public string VideoUrl {  get; set; }
        public string content { get; set; }
        public bool IsApproved { get; set; }

        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public int? SubjectID {  get; set; }
        public Subject subject {  get; set; }
        public int GradeId {  get; set; }
        public Grade grade { get; set; }
        public ICollection<Quiz> quizzes { get; set; }

    }
}

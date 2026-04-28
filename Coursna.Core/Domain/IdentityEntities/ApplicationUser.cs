using Coursna.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.IdentityEntities
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        //hna el admin howa elly by3mel el approve 
        public bool IsApproved { get; set; } = false;

        // hna el student by4of howa tba3 2nhy teacher
        public string? TeacherId {  get; set; }
        public ApplicationUser Teacher { get; set; }

        //el students elly 3and el teacher da 
        public ICollection<ApplicationUser> Students { get; set; }
        public string? InviteCode { get; set; }
        //el courses elly 3nd el teacher
        public ICollection<Course> Courses { get; set; }
        //stendents courses enrollment
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }

        // lma el student ysagel y7ot el grade 
        public int? gradeId {  get; set; }
        public Grade grade { get; set; }
        public int? quizAttemptId { get; set; }
        public QuizAttempt? quizAttempt { get; set; }

    }
}

using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.Entities
{
    public class ApplicationUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public string UserName { get; set; }
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; } // e.g. "Admin", "Teacher", "Student"

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
        public ICollection<QuizAttempt> QuizAttempts { get; set; }

        // lma el student ysagel y7ot el grade 
        public int? gradeId {  get; set; }
        public Grade grade { get; set; }


    }
}



using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class RegisterStudentDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

       // dy 34an 7tt el multi-tenant
        [Required]
        public string InviteCode { get; set; }
        public int GradeId { get; set; }
    }
}


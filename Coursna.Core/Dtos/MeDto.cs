using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class MeDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool IsApproved { get; set; }

        public string? TeacherId { get; set; }

        public string? TeacherName { get; set; }

        public string? InviteCode { get; set; }
    }
}

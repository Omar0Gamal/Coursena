using Coursna.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class TeacherResponseDto
    {
        public string Id {  get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public string InviteCode { get; set; }
        }
    public static class TeacherExtenstion
    {
        public static TeacherResponseDto ToTeacherResponse(this ApplicationUser user)
        {
            return new TeacherResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                InviteCode = user.InviteCode,
                IsApproved = user.IsApproved
            };
        }
    }
}

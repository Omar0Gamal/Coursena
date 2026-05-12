using Coursna.Core.Domain.Entities;


namespace Coursna.Core.Dtos
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int gradeID { get; set; }
    }

    public static class UserResponseDtoExtensions
    {
        public static UserResponseDto ToUserResponseDto(this ApplicationUser user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                gradeID = (user.gradeId == null) ? 0 : (int)user.gradeId
            };
        }
    }
}

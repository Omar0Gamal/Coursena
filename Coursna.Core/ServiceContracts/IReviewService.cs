using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IReviewService
    {
        Task<AuthResponseDto> AddReviewAsync(string studentId, CreateReviewDto dto);
        Task<List<ReviewResponseDto>> GetCourseReviewsAsync(int courseId);
    }
}

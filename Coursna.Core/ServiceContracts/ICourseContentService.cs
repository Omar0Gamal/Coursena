using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface ICourseContentService
    {
       
            Task<AuthResponseDto> AddContentAsync(CreateContentDto dto, string teacherId);
            Task<List<CourseContentResponseDto>> GetCourseContentAsync(int courseId, string studentId);
      
    }
}

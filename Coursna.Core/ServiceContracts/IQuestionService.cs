using Coursna.Core.Domain.Entities;
using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IQuestionService
    {
        Task UpdateQuestionAsync(int questionId, CreateQuestionDto dto, string teacherId);
        Task DeleteQuestionAsync(int questionId, string teacherId);
        Task<int> AddOptionAsync(int questionId, CreateOptionDto request, string teacherId);
        Task UpdateOptionAsync(int optionId, CreateOptionDto request, string teacherId);
        Task DeleteOptionAsync(int optionId, string teacherId);
    }
}

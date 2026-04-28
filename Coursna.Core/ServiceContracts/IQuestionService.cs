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
        Task UpdateQuestionAsync(int questionId, CreateQuestionDto dto);
        Task DeleteQuestionAsync(int questionId);
        Task<int> AddOptionAsync(int questionId, CreateOptionDto request);
        Task UpdateOptionAsync(int optionId, CreateOptionDto request);
        Task DeleteOptionAsync(int optionId);
    }
}

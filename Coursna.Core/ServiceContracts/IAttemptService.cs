using Coursna.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.ServiceContracts
{
    public interface IAttemptService
    {
        Task<int> StartAttemptAsync(int quizId, StartAttemptRequest request);
        Task SaveResponseAsync(int attemptId, SaveResponseRequest request);
        Task<AttemptResultResponse> SubmitAttemptAsync(int attemptId);
    }
}

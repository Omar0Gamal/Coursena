using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Infrastrcuter.BackgroundJobs
{
    public class QuizTimeoutJob: IQuizTimeoutJob
    {
            private readonly IAttemptService _attemptService;

            public QuizTimeoutJob(IAttemptService attemptService)
            {
                _attemptService = attemptService;
            }


        public async Task AutoSubmitAttemptAsync(int attemptId, string studentId)
        {
         
            try
            {
                await _attemptService.SubmitAttemptAsync(attemptId,studentId);
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}

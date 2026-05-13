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
                // We don't want to throw an exception if it's already submitted, 
                // just gracefully finish the job.
                await _attemptService.SubmitAttemptAsync(attemptId, studentId);
            }
            catch (InvalidOperationException)
            {
                // Likely already submitted manually by student
            }
            catch (Exception)
            {
                // Log or handle other background job errors
                throw; // Rethrow to let Hangfire retry if it's a transient failure
            }
        }
    }
}

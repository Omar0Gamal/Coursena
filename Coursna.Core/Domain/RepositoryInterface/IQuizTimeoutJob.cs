using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Domain.RepositoryInterface
{
    public interface IQuizTimeoutJob
    {
        public Task AutoSubmitAttemptAsync(int attemptId, string studentId);

    }
}

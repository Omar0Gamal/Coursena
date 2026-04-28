using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Dtos;
using Coursna.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Service
{
    public class AttemptService : IAttemptService
    {
        private readonly IAttemptRepository _attemptRepo;
        private readonly IQuizRepository _quizRepo;
        private readonly IRepository<QuizAttempt> repository ;
        public AttemptService(IAttemptRepository attemptRepo, IQuizRepository quizRepo)
        {
            _attemptRepo = attemptRepo;
            _quizRepo = quizRepo;
        }

        public async Task SaveResponseAsync(int attemptId, SaveResponseRequest request)
        {
         
        }

        public Task<int> StartAttemptAsync(int quizId, StartAttemptRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AttemptResultResponse> SubmitAttemptAsync(int attemptId)
        {
            throw new NotImplementedException();
        }
    }
}

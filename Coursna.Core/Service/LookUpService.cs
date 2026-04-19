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
    public class LookUpService : ILookUpService
    {
        private readonly IRepository<Subject> _subjectRepo;
        private readonly IRepository<Grade> _gradeRepo;

        public LookUpService(
            IRepository<Subject> subjectRepo,
            IRepository<Grade> gradeRepo)
        {
            _subjectRepo = subjectRepo;
            _gradeRepo = gradeRepo;
        }
        public async Task<List<LookupResponseDto>> GetGradesAsync()
        {
            var grades = await _gradeRepo.GetAllAsync();

            return grades.Select(g => new LookupResponseDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
        }


        public async Task<List<LookupResponseDto>> GetSubjectsAsync()
        {
            var subjects = await _subjectRepo.GetAllAsync();

            return subjects.Select(s => new LookupResponseDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}

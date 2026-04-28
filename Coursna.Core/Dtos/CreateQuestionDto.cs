using Coursna.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class CreateQuestionDto
    {
        public string Title { get; set; }
        public int Points { get; set; }
        public int QuizId { get; set; }
        public List<Option> Options { get; set; }

    }
    public class Options
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}

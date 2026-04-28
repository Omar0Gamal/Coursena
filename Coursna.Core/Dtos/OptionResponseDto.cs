using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class OptionResponseDto
    {
        public int Id {  get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}

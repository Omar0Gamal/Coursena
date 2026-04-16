using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static AuthResponseDto Success(string msg)
            => new() { IsSuccess = true, Message = msg };

        public static AuthResponseDto Fail(string msg)
            => new() { IsSuccess = false, Message = msg };
    }

}

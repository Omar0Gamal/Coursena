using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Core.Dtos
{
    public class ApiResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static ApiResponseDto Success(string msg)
            => new() { IsSuccess = true, Message = msg };

        public static ApiResponseDto Fail(string msg)
            => new() { IsSuccess = false, Message = msg };
    }

}

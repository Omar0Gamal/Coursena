using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursna.Controllers
{
    [ApiController]
    [Route("api/teacher/courses")]
    [Authorize(Roles = "Teacher")]

    public class QuestionController : ControllerBase
    {

        public QuestionController() { 

        }
    }
}

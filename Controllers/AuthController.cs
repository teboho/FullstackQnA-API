using FullstackQnA_API.Extras;
using Microsoft.AspNetCore.Mvc;

namespace FullstackQnA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Authorize(string pass)
        {
            AuthResult result = new AuthResult();

            // Passcode (lol) for admin access
            if (pass.Equals(Program.Passcode))
            {
                result.Authorised = true;
            }
            else
            {
                result.Authorised = false;
            }

            return new JsonResult(result);
        }
    }
}

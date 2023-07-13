using FullstackQnA_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FullstackQnA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass">Query string for logging in</param>
        /// <returns></returns>
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

using Microsoft.AspNetCore.Mvc; 
using CodeDemo.API.AuthenticationService;
using CodeDemo.API.Models; 

namespace CodeDemo.API.Controllers
{
    /// <summary>
    /// User Authentication
    /// </summary>
    [ApiController] 
    public class LoginController : ControllerBase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IAuthService authenticationService;
        /// <summary>
        /// This constructor injects the authentication service
        /// </summary>
        /// <param name="_authenticationService"></param>
        public LoginController(IAuthService _authenticationService)
        {
            authenticationService = _authenticationService;
        }

        /// <summary>
        /// Please generate your JWT Token from this action. Then click on the Authorize button and Value = "Bearer "Token string.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Login user with security Token</returns> 
        [Consumes("application/json")]
        [HttpPost("Authenticate")]
        public IActionResult Login([FromBody]LoginParams user)
        { 
            var loginUser = authenticationService.Authenticate(user.Username, user.Password, out string errorMessage);
            if (loginUser == null)
            {
                logger.Error(errorMessage);
                return Unauthorized(new { Message = errorMessage });
            }
             
            return Ok(loginUser);
        }
    }
}

using JWTTest.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace JWTTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IJWTAuth _jWTAuth;
        public AuthController(IJWTAuth jWTAuth)
        {
            _jWTAuth = jWTAuth;
        }

        [HttpPost]
        public string GenerateJWTToken()
        {
            return _jWTAuth.GenerateToken();
        }
    }
}

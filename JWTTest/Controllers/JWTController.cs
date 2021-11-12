using JWTTest.HeaderValidation;
using JWTTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [HeaderValidator]
    public class JWTController : Controller
    {
        private readonly IJWTService _jwtservice;
        public JWTController(IJWTService jwtservice)
        {
            _jwtservice=jwtservice;
        }
        
        [HttpGet]
        public IActionResult GetX(int num)
        {
            return Ok(_jwtservice.XMethod(num));
        }
    }
}
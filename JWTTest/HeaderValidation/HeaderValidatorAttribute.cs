using JWTTest.Authentication;
using JWTTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace JWTTest.HeaderValidation
{
    public class HeaderValidatorAttribute : Attribute, IAsyncActionFilter
    {
        private ILogger _logger;
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<HeaderValidatorAttribute>>();
            var isAuthenticated = JWTValidationAsync<Response>(context);
            if (!isAuthenticated.SuccessIn)
            {
                context.Result = new UnauthorizedObjectResult(isAuthenticated);
                _logger.LogInformation($"JWT Token validation completed with status: {isAuthenticated}");
                return;
            }
            await next();
        }

        private T JWTValidationAsync<T>(ActionExecutingContext context) where T : new()
        {
            bool isJwtTokenValidated = false;
            var jwtAuthentication = context.HttpContext.RequestServices.GetRequiredService<IJWTAuth>();
            try
            {
                if (context.HttpContext.Request.Headers.TryGetValue("JWT", out StringValues incomingToken))
                {
                    isJwtTokenValidated = jwtAuthentication.ValidatedToken(incomingToken);
                    if (isJwtTokenValidated)
                    {
                        var resp = new Response { SuccessIn = isJwtTokenValidated };
                        return (T)Convert.ChangeType(resp, typeof(T));
                    }
                    else
                    {
                        var resp = new Response { SuccessIn = isJwtTokenValidated, ErrorMessage = "Unauthorized - Invalid JWT Token." };
                        return (T)Convert.ChangeType(resp, typeof(T));
                    }
                }
                else
                {
                    var resp = new Response { SuccessIn = isJwtTokenValidated, ErrorMessage = "JWT Header Value is Mandatory." };
                    return (T)Convert.ChangeType(resp, typeof(T));
                }
            }
            catch (Exception ex)
            {
                var resp = new Response { SuccessIn = isJwtTokenValidated, ErrorMessage = "Exception Occurred." };
                _logger.LogInformation($"Exception Occurred {ex.Message} ");
                return (T)Convert.ChangeType(resp, typeof(T));
            }
            return default;
        }
    }
}

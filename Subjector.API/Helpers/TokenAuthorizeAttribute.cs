using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Subjector.API.Controllers;
using Subjector.API.Models;
using Subjector.Common.Exceptions;

namespace Subjector.API.Helpers
{
    public class TokenAuthorizeAttribute : ActionFilterAttribute
    {
        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!SkipValidation(actionContext))
            {
                var authorizationHeader = actionContext.HttpContext.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");
                if (authorizationHeader.Key == null)
                {
                    throw new AuthenticationException("No Authorization header present");
                }

                // Get token from Authorization header
                string tokenString = authorizationHeader.Value.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(tokenString))
                {
                    throw new AuthenticationException("Authorization header cannot be empty");
                }

                // Validate JWT token
                UserJwtModel user;

                try
                {
                    user = SecurityHelper.Decode<UserJwtModel>(tokenString);
                }
                catch (SignatureVerificationException)
                {
                    throw new AuthenticationException("Invalid token!");
                }

                if (user.ExpirationDate < DateTime.UtcNow)
                {
                    throw new AuthenticationException("Token expired! Please, login again");
                }

                // Validate roles
                if (Roles != null && !Roles.Split(',').ToList().Intersect(user.Roles).Any())
                {
                    throw new AuthenticationException("You do not have permission to access this resource!");
                }

                // Add current user to base controller
                var controller = actionContext.Controller as BaseController;
                if (controller != null) controller.CurrentUser = user;
            }

            base.OnActionExecuting(actionContext);
        }

        private bool SkipValidation(ActionExecutingContext actionContext)
        {
            return actionContext.ActionDescriptor.GetType().GetTypeInfo().GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.Controller.GetType().GetTypeInfo().GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}

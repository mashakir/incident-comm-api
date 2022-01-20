using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Incident.Comm.Integration.Api.Config;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Incident.Comm.Integration.Api.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomValidateAntiForgeryTokenAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;

            var config = context.HttpContext.RequestServices.GetService(typeof(CrossSiteSecuritySection)) as CrossSiteSecuritySection;

            if (config.DisableCrossSiteSecurity)
            {
                return;
            }

            var csrfCookieName = config.CsrfCookieName;
            var sharedSecret = config.SharedSecret;
            var csrfHeaderName = config.CsrfHeaderName;

            //Get guid from header (CMS spits this out in hidden field)
            var guidFromHeader = httpContext.Request.Headers[csrfHeaderName];

            //Get hash of secret and id from cookie (CMS sets cookie with hash of guid + shared secret)
            var hashFromCookie = httpContext.Request.Cookies[csrfCookieName];

            var hashCalculatedFromGuidAndSecretInsideApi = GenerateHash(guidFromHeader, sharedSecret);

            if (hashCalculatedFromGuidAndSecretInsideApi != hashFromCookie)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private static string GenerateHash(string guid, string secret)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(guid + secret);

            var sha256thing = SHA256.Create();
            var hash = sha256thing.ComputeHash(bytes);

            var builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}

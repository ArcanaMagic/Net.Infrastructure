using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Net.Infrastructure.Exceptions;

namespace Net.Infrastructure.Extensions
{
    public static class AuthenticationExtensions
    {
        public static string GetClaimValue(this ClaimsPrincipal user, string claimType)
        {
            var claim = user.Claims.FirstOrDefault(x => x.Type == claimType);

            if(claim == null)
                throw new ApiException($"Claim with type '{claimType}' not found!");

            return claim.Value;
        }

        public static void Validate(this IdentityResult result)
        {
            if (result.Succeeded) 
                return;

            var errors = result.Errors.ToArray();
            var sb = new StringBuilder();
            for (var i = 0; i < errors.Length; i++)
            {
                sb.Append(errors[i].Description);

                if (errors.Length != i + 1)
                {
                    sb.Append("^");
                }
            }
            throw new ValidateException(sb.ToString());
        }

    }
}

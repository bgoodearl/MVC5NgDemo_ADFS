using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace MVCDemo.Infrastructure
{
    public static class ClaimsHelper
    {
        public static Claim GetClaim(this ClaimsPrincipal principal, string claimType)
        {
            if ((principal != null) && (principal.Claims != null) && !string.IsNullOrWhiteSpace(claimType))
            {
                foreach(var claim in principal.Claims)
                {
                    if (claim.Type == claimType)
                        return claim;
                }
            }
            return null;
        }

        public static string GetNameIdentiferValue(this ClaimsPrincipal principal)
        {
            var claim = GetClaim(principal, ClaimTypes.NameIdentifier);
            if (claim != null)
                return claim.Value;
            return null;
        }
    }
}
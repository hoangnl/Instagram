using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Instagram.Helpers
{
    public class UserHelper : IUserHelper
    {
        public string GetCurrentUserIdFromClaim(IPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            string userId = null;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    userId = userIdClaim.Value;
                }
            }
            return userId;
        }
    }
}
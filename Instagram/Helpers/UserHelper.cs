using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Instagram.ViewModel.User;
using Instagram.Service.Feed;

namespace Instagram.Helpers
{
    public class UserHelper : IUserHelper
    {
        private IUserService userService;

        public UserHelper()
        {
            userService = new UserService();
        }
        public UserProfileViewModel GetCurrentProfileUser(string userId1, string userId2)
        {
            UserProfileViewModel userProfile = HttpContext.Current.Session["User"] as UserProfileViewModel;
            if (userProfile == null)
            {
                userProfile = userService.GetUserProfileByUserId(userId1, userId2);
                HttpContext.Current.Session["User"] = userProfile;
            }
            return userProfile;
        }

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
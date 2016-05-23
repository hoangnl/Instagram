using Instagram.ViewModel.Feed;
using Instagram.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Instagram.Helpers
{
    public interface IUserHelper
    {
        string GetCurrentUserIdFromClaim(IPrincipal user);

        UserProfileViewModel GetCurrentProfileUser(string userId1, string userId2);
    }
}
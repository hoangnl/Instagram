using Instagram.ViewModel.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Instagram.Helpers
{
    public interface IUserHelper
    {
        string  GetCurrentUserIdFromClaim(IPrincipal user);
    }
}
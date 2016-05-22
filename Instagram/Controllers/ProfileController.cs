using Instagram.Helpers;
using Instagram.Service.Feed;
using Instagram.ViewModel.User;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Instagram.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly IUserHelper UserHelper;
        private readonly IUserService UserService;
        public ProfileController()
        {
            UserHelper = new UserHelper();
            UserService = new UserService();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Detail(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            var loginUserId = UserHelper.GetCurrentUserIdFromClaim(User);
            var userProfile = UserService.GetUserProfileByUserId(user.Id, loginUserId);
            return View(userProfile);
        }

        public ActionResult Edit()
        {
            string userId = UserHelper.GetCurrentUserIdFromClaim(User);
            var userProfile = UserService.GetUserProfileByUserId(userId, userId);
            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfileViewModel userProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                UserService.SaveUserProfile(userProfileViewModel);
            }
            return RedirectToAction("Edit");
        }
    }
}
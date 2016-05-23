using System.Web.Mvc;
using Instagram.Service.Feed;
using Instagram.ViewModel.Feed;
using System.Collections.Generic;
using System.Web;
using Instagram.Helpers;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.IO;

namespace Instagram.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IFeedService feedService;
        private IUserService userService;
        private IUserHelper userHelper;
        private IFileProcessor fileProcessor;

        public HomeController()
        {
            feedService = new FeedService();
            userService = new UserService();
            userHelper = new UserHelper();
            fileProcessor = new FileProcessor();
        }


        public ActionResult Index()
        {
            //var userId = "f4e550cf-1c85-4908-bdd8-b64eb58d0b06";
            var userId = userHelper.GetCurrentUserIdFromClaim(User);
            bool hasNotFollower = userService.HasNotFollower(userId);
            var userProfile = userHelper.GetCurrentProfileUser(userId, userId);
            ViewBag.UserName = userProfile.UserName;
            ViewBag.Avartar = userProfile.Avartar;
            //if (hasNotFollower)
            //{
            //    return RedirectToAction("Suggestion");
            //}
            //else
            //{
            int pageSize = 5;
            var newsFeed = feedService.GetFeedsPaging(userId, 0, pageSize);
            return View(newsFeed);
            //}
        }

        public ActionResult Suggestion()
        {
            var userId = userHelper.GetCurrentUserIdFromClaim(User);
            bool hasNotFollower = userService.HasNotFollower(userId);
            if (!hasNotFollower)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var userViewModels = userService.GetSuggetions(userId);
                return View(userViewModels);
            }
        }

        public ActionResult GetFeedPaging(int pageIndex = 0)
        {
            //var userId = "f4e550cf-1c85-4908-bdd8-b64eb58d0b06";
            var userId = userHelper.GetCurrentUserIdFromClaim(User);

            int pageSize = 5;
            var newsFeed = feedService.GetFeedsPaging(userId, pageIndex, pageSize);
            if (Request.IsAjaxRequest())
            {
                var userProfile = userHelper.GetCurrentProfileUser(userId, userId);
                ViewBag.UserName = userProfile.UserName;
                ViewBag.Avartar = userProfile.Avartar;
                return PartialView("_FeedView", newsFeed);
            }
            return View("Index", newsFeed);
        }

        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            var users = userService.SearchUser(searchTerm);
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostPhoto(FeedViewModel feed, IEnumerable<HttpPostedFileBase> file)
        {
            if (ModelState.IsValid)
            {
                //foreach (string fileName in Request.Files)
                //{
                //    HttpPostedFileBase file = Request.Files[fileName];
                //}
                //photo = Request.Files;
                IEnumerable<FileViewModel> files = fileProcessor.ProcessFile(file);
                if (files.Count() > 0)
                {
                    //feed.UserId = "f4e550cf-1c85-4908-bdd8-b64eb58d0b06";
                    feed.UserId = userHelper.GetCurrentUserIdFromClaim(User);
                    feedService.SaveFeedAndFile(feed, files);
                }
            }
            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult Like(long feedId)
        {
            //var userId = "f4e550cf-1c85-4908-bdd8-b64eb58d0b06";
            var userId = userHelper.GetCurrentUserIdFromClaim(User);
            bool likeResult = feedService.LikeFeed(userId, feedId);
            var feedLikeSummary = new FeedLikeSummary()
            {
                FeedId = feedId,
                Liked = true,
                TotalLike = feedService.GetFeedTotalLike(feedId)
            };
            return PartialView("_CommandView", feedLikeSummary);
        }

        [HttpPost]
        public ActionResult Unlike(long feedId)
        {
            //var userId = "f4e550cf-1c85-4908-bdd8-b64eb58d0b06";
            var userId = userHelper.GetCurrentUserIdFromClaim(User);
            bool likeResult = feedService.UnlikeFeed(userId, feedId);
            var feedLikeSummary = new FeedLikeSummary()
            {
                FeedId = feedId,
                Liked = false,
                TotalLike = feedService.GetFeedTotalLike(feedId)
            };
            return PartialView("_CommandView", feedLikeSummary);
        }

        [HttpPost]
        public ActionResult Comment(long feedId, string content)
        {
            //var userId = "f4e550cf-1c85-4908-bdd8-b64eb58d0b06";
            var userId = userHelper.GetCurrentUserIdFromClaim(User);
            var feedCommentViewModel = feedService.Comment(feedId, userId, content);
            return PartialView("_CommentView", feedCommentViewModel);
        }

        [HttpPost]
        public ActionResult DeleteComment(long commentId)
        {
            var deleteResult = feedService.DeleteComment(commentId);
            return Json(deleteResult);
        }

        [HttpPost]
        public ActionResult LikeComment(long feedCommentId)
        {
            //var userId = "f4e550cf-1c85-4908-bdd8-b64eb58d0b06";
            var userId = userHelper.GetCurrentUserIdFromClaim(User);
            bool likeResult = feedService.LikeFeed(userId, feedCommentId);
            var feedLikeSummary = new FeedLikeSummary()
            {
                FeedId = feedCommentId,
                Liked = true,
                TotalLike = feedService.GetFeedTotalLike(feedCommentId)
            };
            return PartialView("_CommandView", feedLikeSummary);
        }

        [HttpPost]
        public ActionResult UnlikeComment(long feedCommentId)
        {
            //var userId = "f4e550cf-1c85-4908-bdd8-b64eb58d0b06";
            var userId = userHelper.GetCurrentUserIdFromClaim(User);
            bool likeResult = feedService.LikeFeed(userId, feedCommentId);
            var feedLikeSummary = new FeedLikeSummary()
            {
                FeedId = feedCommentId,
                Liked = true,
                TotalLike = feedService.GetFeedTotalLike(feedCommentId)
            };
            return PartialView("_CommandView", feedLikeSummary);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult GetUserInfo(string userId)
        {
            var user = userService.GetUserInfo(userId);
            return PartialView("_UserInfo", user);
        }

        public ActionResult GetLikedUserList(long feedId)
        {
            IEnumerable<FeedLikeViewModel> likedUserList = userService.GetLikedUserList(feedId);
            return PartialView("_LikedUserList", likedUserList);
        }

        [HttpPost]
        public ActionResult Follow(string userId)
        {
            var userFollowId = userHelper.GetCurrentUserIdFromClaim(User);
            bool followResult = userService.Follow(userId, userFollowId);

            return PartialView("_FollowView", new UserViewModel() { UserId = userId, Following = true });

        }

        [HttpPost]
        public ActionResult Unfollow(string userId)
        {
            var userFollowId = userHelper.GetCurrentUserIdFromClaim(User);
            bool followResult = userService.Unfollow(userId, userFollowId);

            return PartialView("_FollowView", new UserViewModel() { UserId = userId, Following = false });

        }

        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");
                        var fileName1 = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);
                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);
                    }
                }
            }
            catch (System.Exception ex)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        public ActionResult Autocomplete(string searchTerm)
        {
            var users = userService.Autocomplete(searchTerm).Select(
                s => new
                {
                    UserId = s.UserId,
                    FullName = s.FullName,
                    Avartar = Url.Content(s.Avartar),
                    UserName = s.UserName
                }
                );
            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}
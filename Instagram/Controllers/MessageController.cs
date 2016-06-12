using Instagram.Helpers;
using Instagram.Service.Feed;
using Instagram.Service.Message;
using Instagram.ViewModel.Feed;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Instagram.Controllers
{
    public class MessageController : Controller
    {
        private IMessageService messageService;
        private IUserHelper userHelper;
        private IUserService userService;

        public MessageController()
        {
            messageService = new MessageService();
            userHelper = new UserHelper();
            userService = new UserService();
        }

        public MessageController(IMessageService _messageService, IUserHelper _userHelper)
        {
            messageService = _messageService;
            userHelper = _userHelper;
        }

        // GET: Message
        public ActionResult Index()
        {
            var userId = userHelper.GetCurrentUserIdFromClaim(User);
            int pageSize = 5;
            var messageWrapperViewModel = messageService.GetPagingMessage(userId, 0, pageSize);
            ViewBag.ActiveUserName = messageWrapperViewModel.Users.FirstOrDefault().FullName;
            ViewBag.ActiveUserId = messageWrapperViewModel.Users.FirstOrDefault().UserId;
            return View(messageWrapperViewModel);
        }

        [HttpPost]
        public ActionResult SelectMessage(string userId, string fullName)
        {
            IEnumerable<MessageViewModel> messageViewModels = messageService.GetMessageByUserId(userHelper.GetCurrentUserIdFromClaim(User), userId);
            ViewBag.ActiveUserName = fullName;
            ViewBag.ActiveUserId = userId;
            return PartialView("_MessageListView", messageViewModels);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUsers(string term)
        {
            var result = new List<KeyValuePair<string, string>>();
            IEnumerable<UserViewModel> users = userService.SearchUser(term);
            foreach (var user in users) {
                result.Add(new KeyValuePair<string, string>(user.UserId, user.FullName)); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendMessage(string toUserId, string message)
        {
            string UserId = userHelper.GetCurrentUserIdFromClaim(User);
            long result = messageService.SendMessage(UserId, toUserId, message);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
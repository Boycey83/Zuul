using System;
using System.Web.Mvc;
using Zuul.BusinessLogic.Services;
using Zuul.Web.Models;

namespace Zuul.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserAccountService _userAccountService;

        public HomeController(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public ActionResult Index()
        {
            return View(new ForumViewModel());
        }

        [Route("Thread/{threadId:int}")]
        public ActionResult Index(int threadId)
        {
            return View(new ForumViewModel { ThreadId = threadId });
        }

        [Route("Thread/{threadId:int}/Reply/{replyId:int}")]
        public ActionResult Index(int threadId, int replyId)
        {
            return View(new ForumViewModel { ThreadId = threadId, ReplyId = replyId });
        }

        [Route("UserAccount/{userAccountId:int}/Confirm/{tokenString}")]
        public ActionResult ConfirmUserAccount(int userAccountId, string tokenString)
        {
            var token = Guid.Parse(tokenString);
            _userAccountService.ActivateUser(userAccountId, token);
            return RedirectToAction("Index");
        }
    }
}
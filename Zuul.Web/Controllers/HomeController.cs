using System;
using System.Web.Mvc;
using Zuul.BusinessLogic.Services;

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
            return View();
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingTool.Areas.AccountingData.Controllers
{
    [Area("AccountingData")]
    public class AccountPageController : Controller
    {
        public ActionResult CreateAccount()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult UnauthorizedPage()
        {
            return View();
        }
    }
}

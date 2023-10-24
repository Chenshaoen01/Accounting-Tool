using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingTool.Areas.AccountingData.Controllers
{
    [Area("AccountingData")]
    public class CreateAndUpdateController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingTool.Areas.AccountingData.Controllers
{
    [Area("AccountingData")]
    public class CreateAndUpdateController : Controller
    {
        public ActionResult Create()
        {
            return View("Index");
        }

        public ActionResult Edit(int Id)
        {
            ViewBag.Id = Id;
            return View("Index");
        }
    }
}

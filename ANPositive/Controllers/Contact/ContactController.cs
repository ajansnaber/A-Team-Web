using System.Web.Mvc;
using ANPositive.Models;
using ANPositive.Models.Shared;

namespace ANPositive.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Contact";
            return View();
        }

        public JsonResult Send(Email model)
        {

            Result result = new Result
            {
                success = true,
                title = "Yeni İçerik",
                message = $"Sayıın {model.firstNameLastName}. Mesajınız bize ulaştı. En kısa sürede sizinle iletişime geçeceğiz."
            };

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }
    }
}
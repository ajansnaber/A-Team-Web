using System.Web.Mvc;

namespace ANPositive.Controllers
{
    [Route("tr/[controller]")]
    public class IletisimController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "İletişim";

            return View();
        }
    }
}
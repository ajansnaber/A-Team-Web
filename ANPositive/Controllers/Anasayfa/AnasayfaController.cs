using System.Web.Mvc;

namespace ANPositive.Controllers
{
    [Route("tr/[controller]")]
    public class AnasayfaController : Controller
    {
        public ActionResult index()
        {
            ViewBag.Title = "Anasayfa";
            return View();
        }
    }
}
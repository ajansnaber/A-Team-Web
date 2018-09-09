using ANPositive.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANPositive.Controllers
{
    [Route("en/[controller]")]
    public class ContentsController : Controller
    {

        /// <summary>
        ///     Context
        /// </summary>
        private ApplicationDbContext _dbContext;

        public ContentsController()
        {
        }

        public ContentsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApplicationDbContext DbContext
        {
            get => _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            private set => _dbContext = value;
        }

        public ActionResult Index(int? id)
        {
            Content content = DbContext.contents.FirstOrDefault(item => item.id == id);

            if (content == null)
                return RedirectToAction("Index", "Home");

            ViewBag.Title = content.title;

            return View(content);
        }
    }
}
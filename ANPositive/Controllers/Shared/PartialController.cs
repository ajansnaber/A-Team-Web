using ANPositive.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANPositive.Controllers
{
    public class PartialController : Controller
    {
        /// <summary>
        /// Context
        /// </summary>
        private ApplicationDbContext _dbContext;

        public ApplicationDbContext DbContext
        {
            get
            {
                return _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _dbContext = value;
            }

        }

        public PartialController()
        {

        }

        public PartialController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult NavigationMenu(int menuPosition, int lang)
        {
            List<Content> contents = DbContext.contents.Where(item => item.published && !item.isDeleted && item.language == lang && item.menuPosition == menuPosition).OrderBy(item => item.displayOrder).ToList();

            return PartialView("~/Views/Shared/_Navigation.cshtml", contents);
        }
    }
}
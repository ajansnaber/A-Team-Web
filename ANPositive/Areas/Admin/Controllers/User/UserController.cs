using System;
using ANPositive.Models;
using ANPositive.Models.Shared;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ANPositive.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
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

        public UserController()
        {

        }

        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Default Action for Users
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        /// <summary>
        /// Users List
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            ViewBag.TopMenu = "Kullanıcı Yönetimi";
            ViewBag.Title = "Kullanıcılar";

            return View();
        }

        /// <summary>
        /// Insert Content to Users
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert()
        {
            ViewBag.TopMenu = "Kullanıcı Yönetimi";
            ViewBag.Title = "Yeni Kullanıcı Ekle";

            return View();
        }

        /// <summary>
        /// CRUD Operation For Users
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            ViewBag.TopMenu = "Kullanıcı Yönetimi";
            ViewBag.Title = "Kullanıcı Düzenle";

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store: new UserStore<ApplicationUser>(DbContext));
            ApplicationUser user = userManager.FindById(id);

            if (user == null)
                RedirectToAction("List");

            return View(user);
        }

        public JsonResult create(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store: new UserStore<ApplicationUser>(DbContext));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                };

                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(store: new RoleStore<IdentityRole>(DbContext));

                ApplicationUser user = new ApplicationUser
                {
                    title = model.title,
                    Email = model.Email,
                    UserName = model.Email,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    EmailConfirmed = true,
                    TwoFactorEnabled = true,
                };

                IdentityResult userResult = userManager.Create(user, "P@r0la..");
                if (userResult.Succeeded)
                    userManager.AddToRole(user.Id, RoleNames.roleEditor);

                Result result = new Result
                {
                    success = true,
                    title = "Yeni Kullanıcı",
                    message = $"{user.Email} eposta adresli kullanıcı başarılı bir şekilde eklendi."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Result result = new Result
                {
                    success = false,
                    title = "Yeni Kullanıcı",
                    message = "Yeni kullanıcı eklenirken bir hata oluştu. "
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult update(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                using (DbContext)
                {
                    ApplicationUser applicationUser = DbContext.Users.FirstOrDefault(x => x.Id == model.Id);

                    if (applicationUser != null)
                    {
                        applicationUser.firstName = model.firstName;
                        applicationUser.lastName = model.lastName;
                        applicationUser.Email = model.Email;
                        applicationUser.UserName = model.UserName;
                        applicationUser.title = model.title;

                        DbContext.SaveChanges();
                    }

                    Result result = new Result
                    {
                        success = applicationUser != null ? true : false,
                        title = "Kullanıcı Düzenle",
                        message = applicationUser != null ? $"{applicationUser.Email} eposta adresli kullanıcı başarılı bir şekilde düzenlendi." : "Kullanıcı düzenlenirken bir hata oluştu."
                    };

                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Result result = new Result
                {
                    success = false,
                    title = "Kullanıcı Düzenle",
                    message = "Kullanıcı düzenlenirken bir hata oluştu."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult delete(ApplicationUser model)
        {
            using (DbContext)
            {
                ApplicationUser applicationUser = DbContext.Users.FirstOrDefault(x => x.Id == model.Id);

                if (applicationUser != null)
                {
                    DbContext.Users.Remove(applicationUser);
                    DbContext.SaveChanges();
                }

                Result result = new Result
                {
                    success = applicationUser != null ? true : false,
                    title = "Kullanıcı Sil",
                    message = applicationUser != null ? $"{applicationUser.Email} eposta adresli kullanıcı başarılı bir şekilde silindi." : "Kullanıcı silinirken bir hata oluştu."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
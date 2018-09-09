using System;
using ANPositive.Models;
using ANPositive.Models.Shared;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ANPositive.Areas.Admin.Controllers
{
    [Authorize]
    public class ContentController : Controller
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

        public ContentController()
        {

        }

        public ContentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Default Action for Pages
        /// </summary>
        /// <returns></returns>
        public ActionResult index()
        {
            return RedirectToAction("List");
        }

        /// <summary>
        /// Pages List
        /// </summary>
        /// <returns></returns>
        public ActionResult list()
        {
            ViewBag.TopMenu = "İçerik Yönetimi";
            ViewBag.Title = "İçerikler";

            return View();
        }

        /// <summary>
        /// Insert Content to Pages
        /// </summary>
        /// <returns></returns>
        public ActionResult insert()
        {
            ViewBag.TopMenu = "İçerik Yönetimi";
            ViewBag.Title = "Yeni İçerik Ekle";

            return View();
        }

        /// <summary>
        /// CRUD Operation For Pages
        /// </summary>
        /// <returns></returns>
        public ActionResult edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            ViewBag.TopMenu = "İçerik Yönetimi";
            ViewBag.Title = "İçerik Düzenle";

            Content content = DbContext.contents.FirstOrDefault(x => x.id == id);
            return View(content);
        }

        public JsonResult create(Content model)
        {
            if (ModelState.IsValid)
            {
                using (DbContext)
                {
                    Content content = new Content
                    {
                        language = model.language,
                        title = model.title,
                        menuPosition = model.menuPosition,
                        body = model.body,
                        image = model.image,
                        metaTitle = model.metaTitle,
                        metaDescription = model.metaDescription,
                        metaTags = model.metaTags,
                        published = true,
                        displayOrder = 32768,
                        createdAt = DateTime.Now,
                        modifiedBy = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        modifiedAt = DateTime.Now,
                        isDeleted = false
                    };

                    DbContext.contents.Add(content);
                    DbContext.SaveChanges();

                    Result result = new Result
                    {
                        success = true,
                        title = "Yeni İçerik",
                        message = $"{model.title} başlıklı içerik başarılı bir şekilde eklendi."
                    };

                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Result result = new Result
                {
                    success = false,
                    title = "Yeni İçerik",
                    message = "Yeni içerik eklenirken bir hata oluştu. "
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult update(Content model)
        {
            if (ModelState.IsValid)
            {
                using (DbContext)
                {
                    Content content = DbContext.contents.FirstOrDefault(x => x.id == model.id);

                    if (content != null)
                    {
                        content.language = model.language;
                        content.title = model.title;
                        content.menuPosition = model.menuPosition;
                        content.body = model.body;
                        content.image = model.image;
                        content.metaTitle = model.metaTitle;
                        content.metaDescription = model.metaDescription;
                        content.metaTags = model.metaTags;
                        content.modifiedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                        content.modifiedAt = DateTime.Now;

                        DbContext.SaveChanges();
                    }

                    Result result = new Result
                    {
                        success = content != null ? true : false,
                        title = "İçerik Düzenle",
                        message = content != null ? $"{content.title} başlıklı içerik başarılı bir şekilde düzenlendi." : "İçerik düzenlenirken bir hata oluştu."
                    };

                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Result result = new Result
                {
                    success = false,
                    title = "İçerik Düzenle",
                    message = "İçerik düzenlenirken bir hata oluştu."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult order(Content model, ICollection<displayOrders> displayOrders)
        {
            using (DbContext)
            {
                foreach (displayOrders displayOrder in displayOrders)
                {
                    DbContext.contents.FirstOrDefault(x => x.id == displayOrder.id).displayOrder =
                        displayOrder.displayOrder;
                    DbContext.SaveChanges();
                }

                Result result = new Result
                {
                    success = true,
                    title = "İçerik Sıralaması",
                    message = "İçerik sıralaması başarılı bir şekilde değiştirildi."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult delete(Content model)
        {
            using (DbContext)
            {
                Content content = DbContext.contents.FirstOrDefault(x => x.id == model.id);

                if (content != null)
                {
                    content.isDeleted = model.isDeleted;
                    content.modifiedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    content.modifiedAt = DateTime.Now;
                    DbContext.SaveChanges();
                }

                Result result = new Result
                {
                    success = content != null ? true : false,
                    title = "Content Sil",
                    message = content != null ? $"{content.title} başlıklı içerik başarılı bir şekilde silindi." : "İçerik silinirken bir hata oluştu."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult publish(Content model)
        {
            using (DbContext)
            {
                Content content = DbContext.contents.FirstOrDefault(x => x.id == model.id);

                if (content != null)
                {
                    content.published = model.published;
                    content.modifiedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    content.modifiedAt = DateTime.Now;
                    DbContext.SaveChanges();
                }

                Result result = new Result
                {
                    success = content != null ? true : false,
                    title = "İçerik Yayınla",
                    message = content != null ? $"{content.title} başlıklı içerik başarılı bir şekilde " + (model.published ? "yayınlandı" : "yayından kaldırıldı") + "." : "İçerik durumu değiştirilirken bir hata oluştu."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
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
    public class GalleryController : Controller
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

        public GalleryController()
        {

        }

        public GalleryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Default Action for Galleries
        /// </summary>
        /// <returns></returns>
        public ActionResult index()
        {
            return RedirectToAction("List");
        }

        /// <summary>
        /// Galleries List
        /// </summary>
        /// <returns></returns>
        public ActionResult list()
        {
            ViewBag.TopMenu = "Galeri Yönetimi";
            ViewBag.Title = "Galeriler";

            return View();
        }

        /// <summary>
        /// Insert Gallery to Galleries
        /// </summary>
        /// <returns></returns>
        public ActionResult insert()
        {
            ViewBag.TopMenu = "Galeri Yönetimi";
            ViewBag.Title = "Yeni Galeri Ekle";

            return View();
        }

        /// <summary>
        /// CRUD Operation For Galleries
        /// </summary>
        /// <returns></returns>
        public ActionResult edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            ViewBag.TopMenu = "Galeri Yönetimi";
            ViewBag.Title = "Galeri Düzenle";

            Gallery gallery = DbContext.galleries.FirstOrDefault(x => x.id == id);
            return View(gallery);
        }

        public JsonResult create(Gallery model)
        {
            if (ModelState.IsValid)
            {
                using (DbContext)
                {
                    Gallery gallery = new Gallery
                    {
                        language = model.language,
                        title = model.title,
                        images = model.images,
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

                    DbContext.galleries.Add(gallery);
                    DbContext.SaveChanges();

                    Result result = new Result
                    {
                        success = true,
                        title = "Yeni Galeri",
                        message = $"{model.title} başlıklı galeri başarılı bir şekilde eklendi."
                    };

                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Result result = new Result
                {
                    success = false,
                    title = "Yeni Galeri",
                    message = "Yeni galeri eklenirken bir hata oluştu. "
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult update(Gallery model)
        {
            if (ModelState.IsValid)
            {
                using (DbContext)
                {
                    Gallery gallery = DbContext.galleries.FirstOrDefault(x => x.id == model.id);

                    if (gallery != null)
                    {
                        gallery.language = model.language;
                        gallery.title = model.title;
                        gallery.images = model.images;
                        gallery.metaTitle = model.metaTitle;
                        gallery.metaDescription = model.metaDescription;
                        gallery.metaTags = model.metaTags;
                        gallery.modifiedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                        gallery.modifiedAt = DateTime.Now;

                        DbContext.SaveChanges();
                    }

                    Result result = new Result
                    {
                        success = gallery != null ? true : false,
                        title = "Galeri Düzenle",
                        message = gallery != null ? $"{gallery.title} başlıklı galeri başarılı bir şekilde düzenlendi." : "Galeri düzenlenirken bir hata oluştu."
                    };

                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Result result = new Result
                {
                    success = false,
                    title = "Galeri Düzenle",
                    message = "Galeri düzenlenirken bir hata oluştu."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult order(Gallery model, ICollection<displayOrders> displayOrders)
        {
            using (DbContext)
            {
                foreach (displayOrders displayOrder in displayOrders)
                {
                    DbContext.galleries.FirstOrDefault(x => x.id == displayOrder.id).displayOrder =
                        displayOrder.displayOrder;
                    DbContext.SaveChanges();
                }

                Result result = new Result
                {
                    success = true,
                    title = "Galeri Sıralaması",
                    message = "Galeri sıralaması başarılı bir şekilde değiştirildi."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult delete(Gallery model)
        {
            using (DbContext)
            {
                Gallery gallery = DbContext.galleries.FirstOrDefault(x => x.id == model.id);

                if (gallery != null)
                {
                    gallery.isDeleted = model.isDeleted;
                    gallery.modifiedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    gallery.modifiedAt = DateTime.Now;
                    DbContext.SaveChanges();
                }

                Result result = new Result
                {
                    success = gallery != null ? true : false,
                    title = "Gallery Sil",
                    message = gallery != null ? $"{gallery.title} başlıklı galeri başarılı bir şekilde silindi." : "Galeri silinirken bir hata oluştu."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult publish(Gallery model)
        {
            using (DbContext)
            {
                Gallery gallery = DbContext.galleries.FirstOrDefault(x => x.id == model.id);

                if (gallery != null)
                {
                    gallery.published = model.published;
                    gallery.modifiedBy = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    gallery.modifiedAt = DateTime.Now;
                    DbContext.SaveChanges();
                }

                Result result = new Result
                {
                    success = gallery != null ? true : false,
                    title = "Galeri Yayınla",
                    message = gallery != null ? $"{gallery.title} başlıklı galeri başarılı bir şekilde " + (model.published ? "yayınlandı" : "yayından kaldırıldı") + "." : "Galeri durumu değiştirilirken bir hata oluştu."
                };

                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
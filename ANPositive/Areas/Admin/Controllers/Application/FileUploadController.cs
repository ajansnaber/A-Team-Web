using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ANPositive.Areas.Admin.Controllers
{
    public class FileUploadController : Controller
    {
        FilesHelper filesHelper;
        String tempPath = "~/uploads/";
        String serverMapPath = "~/uploads/";

        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }

        private string UrlBase = "/uploads/";

        String DeleteURL = "/Admin/FileUpload/DeleteFile/?file=";
        String DeleteType = "GET";

        public FileUploadController()
        {
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);
        }

        public ActionResult Show()
        {
            string subFolder = Request.QueryString["subFolder"] != null ? HttpContext.Request.QueryString["subFolder"].ToString() : "user-files";
            JsonFiles ListOfFiles = filesHelper.GetFileList(subFolder);
            var model = new FilesViewModel()
            {
                Files = ListOfFiles.files
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Upload()
        {
            var resultList = new List<ViewDataUploadFilesResult>();
            var CurrentContext = HttpContext;
            string subFolder = Request.QueryString["subFolder"] != null ? CurrentContext.Request.QueryString["subFolder"].ToString() : "user-files";

            filesHelper.UploadAndShowResults(CurrentContext, resultList, subFolder);

            JsonFiles files = new JsonFiles(resultList);

            bool isEmpty = !resultList.Any();
            if (isEmpty)
            {
                return Json("Error ");
            }
            else
            {
                return Json(files);
            }
        }

        public JsonResult GetFileList()
        {
            string subFolder = Request.QueryString["subFolder"] != null ? HttpContext.Request.QueryString["subFolder"].ToString() : "user-files";
            var list=filesHelper.GetFileList(subFolder);
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteFile(string file, string subFolder)
        {
            filesHelper.DeleteFile(file, subFolder);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
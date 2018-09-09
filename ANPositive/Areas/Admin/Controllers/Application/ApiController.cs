using ANPositive.Models;
using ANPositive.Models.Shared;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ANPositive.Areas.Admin.Controllers
{
    [Authorize]
    public class ApiController : Controller
    {
        private ApplicationDbContext _dbContext;

        public ApiController()
        {
        }

        public ApiController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApplicationDbContext DbContext
        {
            get => _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            private set => _dbContext = value;
        }

        [ActionName("Auto-Complete")]
        public JsonResult autoComplete(string tableName, string columnName, string query)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {0} LIKE '%{2}%' GROUP BY {0};", columnName,
                tableName, query.Replace("'", "''"));

            using (DbContext)
            {
                string[] data = DbContext.Database
                    .SqlQuery<string>(sql).ToArray();

                List<AutoComplete> suggestions = new List<AutoComplete>();

                foreach (var item in data)
                    suggestions.Add(new AutoComplete
                    {
                        value = item
                    });

                return Json(new {suggestions}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUploadedFile(int id, string tableName, string columnName, string subFolder)
        {
            string serverMapPath = "/uploads/" + subFolder + "/";
            string sql = $"SELECT {columnName} FROM {tableName} WHERE  id = {id};";

            using (DbContext)
            {
                string data = DbContext.Database
                    .SqlQuery<string>(sql).ToArray().FirstOrDefault();

                List<ViewDataUploadFilesResult> resultList = new List<ViewDataUploadFilesResult>();

                foreach (string file in data.Split(','))
                {
                    string fileName = Path.GetFileNameWithoutExtension(serverMapPath + file);

                    resultList.Add(new ViewDataUploadFilesResult()
                    {
                        url = serverMapPath + file,
                        deleteType = "GET",
                        deleteUrl = Url.Action("DeleteFile", "FileUpload", new {file = file}),
                        name = file,
                        size = Convert.ToInt32(new FileInfo(Server.MapPath(serverMapPath + file)).Length),
                        thumbnailUrl = serverMapPath + "thumbs/" + fileName + "_thumbs" + ".jpg",
                        type = MimeMappingStealer.GetMimeMapping(serverMapPath + file)
                    });
                }

                JsonFiles files = new JsonFiles(resultList);
                return Json(files, JsonRequestBehavior.AllowGet);
            }
        }

        public static class MimeMappingStealer
        {
            // The get mime mapping method
            private static readonly Func<string, string> _getMimeMappingMethod = null;

            /// <summary>
            /// Static constructor sets up reflection.
            /// </summary>
            static MimeMappingStealer()
            {
                // Load hidden mime mapping class and method from System.Web
                var assembly = Assembly.GetAssembly(typeof(HttpApplication));
                Type mimeMappingType = assembly.GetType("System.Web.MimeMapping");
                _getMimeMappingMethod = (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), mimeMappingType.GetMethod("GetMimeMapping",
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.FlattenHierarchy));
            }

            /// <summary>
            /// Exposes the hidden Mime mapping method.
            /// </summary>
            /// <param name="fileName">The file name.</param>
            /// <returns>The mime mapping.</returns>
            public static string GetMimeMapping(string fileName)
            {
                return _getMimeMappingMethod(fileName);
            }
        }

        [Authorize(Roles = RoleNames.roleAdministrator)]
        public ActionResult users(IDataTablesRequest request)
        {
            var getRole = (from r in DbContext.Roles where r.Name.Contains(RoleNames.roleAdministrator) select r).FirstOrDefault();
            IQueryable<ApplicationUser> data = DbContext.Users.Where(x => !x.Roles.Select(y => y.RoleId).Contains(getRole.Id));
            IQueryable<ApplicationUser> filteredData = data.Where(item =>
                item.UserName.Contains(request.Search.Value) || item.firstName.Contains(request.Search.Value) ||
                item.lastName.Contains(request.Search.Value));
            IQueryable<ApplicationUser> dataPage = filteredData.OrderBy(x => x.UserName).ThenBy(x => x.lastName)
                .Skip(request.Start).Take(request.Length);

            DataTablesResponse response =
                DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult contents(IDataTablesRequest request)
        {
            IQueryable<Content> data = User.IsInRole(RoleNames.roleAdministrator) ? DbContext.contents : DbContext.contents.Where(item => !item.isDeleted);
            IQueryable<Content> filteredData = data.Where(item => item.title.Contains(request.Search.Value) || item.body.Contains(request.Search.Value));
            IQueryable<Content> dataPage = filteredData.OrderBy(x => x.displayOrder).ThenBy(x => x.createdAt).ThenBy(x => x.title).Skip(request.Start).Take(request.Length);

            DataTablesResponse response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult galleries(IDataTablesRequest request)
        {
            IQueryable<Gallery> data = User.IsInRole(RoleNames.roleAdministrator) ? DbContext.galleries : DbContext.galleries.Where(item => !item.isDeleted);
            IQueryable<Gallery> filteredData = data.Where(item => item.title.Contains(request.Search.Value));
            IQueryable<Gallery> dataPage = filteredData.OrderBy(x => x.displayOrder).ThenBy(x => x.createdAt).ThenBy(x => x.title).Skip(request.Start).Take(request.Length);

            DataTablesResponse response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }
    }
}
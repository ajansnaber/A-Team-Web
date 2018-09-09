using ANPositive.Helpers;
using ANPositive.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ANPositive
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            ApplicationDbContext context = new ApplicationDbContext();
            IdentityHelper.SeedIdentities(context);
            DataTables.AspNet.Mvc5.Configuration.RegisterDataTables();
        }
    }
}

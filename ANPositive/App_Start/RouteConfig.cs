using System.Web.Mvc;
using System.Web.Routing;
using ANPositive.Models;

namespace ANPositive
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("jsnlog.logger/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "Contact",
                defaults: new { controller = "Contact", action = "Index" }
            );

            routes.MapRoute(
                name: "Anasayfa",
                url: "tr",
                defaults: new { controller = "Anasayfa", action = "Index" }
            );

            routes.MapRoute(
                name: "Iletisim",
                url: "tr/iletisim",
                defaults: new { controller = "Iletisim", action = "Index" }
            );

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.Add("SEO",
                new SeoFriendlyRoute("{controller}/{id}",
                    new RouteValueDictionary(new { controller = "Content", action = "Index", id = UrlParameter.Optional }),
                    new MvcRouteHandler())
            );

            routes.MapRoute(
                name: "Default",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, lang = UrlParameter.Optional },
                namespaces: new[] { "ANPositive.Controllers" }
            );
        }
    }
}

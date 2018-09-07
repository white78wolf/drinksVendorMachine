using System.Web.Mvc;
using System.Web.Routing;

namespace DrinksVendorMachine
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
            );

            routes.MapRoute(
                name: "OddMoney",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "OddMoney" }
            );

            routes.MapRoute(
                name: "DropCoin",
                url: "{controller}/{action}/{nominal}",
                defaults: new { controller = "Home", action = "DropCoin" }
            );

            routes.MapPageRoute("admin", "admin", "~/Views/Admin/Admin.cshtml");

            routes.MapRoute("logon", "login", "~/Views/Logon/Login.cshtml");
        }
    }
}
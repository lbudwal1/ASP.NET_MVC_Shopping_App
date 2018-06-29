using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shppoing_Application_With_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            // Shppoing_Application_With_MVC.Controllers this is namespace in PagesController 
            routes.MapRoute("Cart", "Cart/{action}/{id}", new { Controller = "Cart", action = "Index", id = UrlParameter.Optional }, new[] { "Shppoing_Application_With_MVC.Controllers" });

            routes.MapRoute("Shop", "Shop/{action}/{name}", new { Controller = "Shop", action = "Index", name = UrlParameter.Optional }, new[] { "Shppoing_Application_With_MVC.Controllers" });


            routes.MapRoute("PagesMenupartical", "pages/PagesMenupartical", new { Controller = "Pages", action = "PagesMenupartical" }, new[] { "Shppoing_Application_With_MVC.Controllers" });
            routes.MapRoute("SidebarPartial", "pages/SidebarPartial", new { Controller = "Pages", action = "SidebarPartial" }, new[] { "Shppoing_Application_With_MVC.Controllers" });

            routes.MapRoute("Pages", "{page}", new { Controller = "Pages", action = "Index" }, new[] { "Shppoing_Application_With_MVC.Controllers" });
            routes.MapRoute("Default", "", new { Controller = "Pages", action = "Index" }, new[] { "Shppoing_Application_With_MVC.Controllers" });

            // i comment this one just becuase i create my own contoller in views pages index

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}

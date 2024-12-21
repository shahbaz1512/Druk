using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IMPSTransactionRouter
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Balanceinquiry",
                url: "api/{MobileBanking}/{Balanceinquiry}/{_MOBILEBANKING_REQ}",
                defaults: new { controller = "MobileBanking", action = "Balanceinquiry" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MobileBanking", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
using StatCoach.Business.RouteConstraints;
using StatCoach.Business.RouteHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StatCoach
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DynamicEntity",
                url: "{club}/{content}/{controller}/{action}/{id}",
                defaults: new { controller = "Club", action = "Index", id = UrlParameter.Optional },
                constraints: new { isClub = new ClubRouteConstraint() }
            ).RouteHandler = new ApplicationRouteHandler();

            routes.MapRoute(
                name: "Club",
                url: "{club}/{controller}/{action}/{id}",
                defaults: new { controller = "Club", action = "Index", id = UrlParameter.Optional },
                constraints: new { isClub = new ClubRouteConstraint() }
            ).RouteHandler = new ApplicationRouteHandler();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );            
        }
    }
}
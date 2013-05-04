using StatCoach.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace StatCoach.Business.RouteConstraints
{
    public class ClubRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.IncomingRequest)
            {
                string club = values["club"].ToString().ToLower();
                using (StatsRepository db = new StatsRepository())
                {
                    if (db.GetClubs().Exists(c => c.Name.Replace(' ', '-').ToLower() == club))
                        return true;
                }
            }

            return false;
        }
    }
}
using StatCoach.Business.Enums;
using StatCoach.Business.Interfaces;
using StatCoach.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StatCoach.Business.RouteHandlers
{
    public class ApplicationRouteHandler : IRouteHandler, IDisposable
    {
        StatsRepository _statsRepository;

        public ApplicationRouteHandler()
        {
            this._statsRepository = new StatsRepository();
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string clubName = requestContext.RouteData.Values["club"] as string ?? string.Empty;
            string contentName = requestContext.RouteData.Values["content"] as string ?? string.Empty;
            string controllerName = requestContext.RouteData.Values["controller"] as string ?? string.Empty;
            string actionName = requestContext.RouteData.Values["action"] as string ?? string.Empty;

            IContent content = null;

            if (clubName != string.Empty)
            {
                Club club = RequestData.Club = this._statsRepository.GetClubBySEOName(clubName);

                if (contentName != string.Empty)
                {
                    content = RequestData.Content = this._statsRepository.GetContentFromRoute(club.Id, contentName);
                }
            }

            if (content != null && controllerName == "Club" && actionName == "Index")
            {
                switch ((ContentType)content.Type)
                {
                    case ContentType.Club:
                        break;
                    case ContentType.Team:
                        requestContext.RouteData.Values["Controller"] = "Team";
                        requestContext.RouteData.Values["Action"] = "Index";
                        break;
                    case ContentType.Player:
                        break;
                    default:
                        break;
                }
            }
            else if (controllerName != string.Empty && controllerName != "Club")
            {

            }
            else
            {
                requestContext.RouteData.Values["Controller"] = "Club";
                requestContext.RouteData.Values["Action"] = "Index";
            }

            return new MvcHandler(requestContext);
        }

        public void Dispose()
        {
            this._statsRepository.Dispose();
        }
    }
}
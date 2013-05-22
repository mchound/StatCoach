using StatCoach.Data;
using StatCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace StatCoach.Helpers
{
    public static class HtmlHelpers
    {
        public static string CurrentUserDisplayName(this HtmlHelper helper)
        {
            using (UserRepository db = new UserRepository())
            {
                UserModel user = db.GetCurrentUser();
                
                if(user != null)
                    return string.Concat(user.FirstName, " ", user.LastName);
                return string.Empty;                
            }
        }

        public static MvcHtmlString ContentAction(this HtmlHelper helper, string action, string linkText)
        {
            string url = BuildUrl(helper.ViewContext.RequestContext, action, helper.ViewContext.RouteData.Values);

            return BuildTag(url, linkText);

        }

        public static MvcHtmlString ContentAction(this HtmlHelper helper, string action, string linkText, object routeValues)
        {
            string url = BuildUrl(helper.ViewContext.RequestContext, action, helper.ViewContext.RouteData.Values);
            url = string.Concat(url, "?");
            int count = 0;

            foreach (var prop in routeValues.GetType().GetProperties())
            {
                url = string.Concat(url, 
                    count > 0 ? "&" : string.Empty, 
                    prop.Name, "=", 
                    prop.GetValue(routeValues));
                count++;
            }

            if(count == 0)
                url = url.Replace("?", string.Empty);

            return BuildTag(url, linkText);
        }

        private static string BuildUrl(RequestContext requestContext, string action, RouteValueDictionary RouteData)
        {
            UrlHelper urlHelper = new UrlHelper(requestContext);
            string url = urlHelper.Action(action);

            string club = RouteData["club"].ToString();

            if (string.IsNullOrWhiteSpace(club))
                return url;

            char separator = '/';
            string content = RouteData["content"].ToString();

            if (string.IsNullOrWhiteSpace(content))
                return string.Concat(separator, club, url);                

            return string.Concat(separator, club, separator, content, url);            
        }

        private static MvcHtmlString BuildTag(string url, string linkText)
        {
            TagBuilder tagBuilder = new TagBuilder("a");            
            tagBuilder.SetInnerText(linkText);
            tagBuilder.Attributes.Add("href", url);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }
        
    }
}
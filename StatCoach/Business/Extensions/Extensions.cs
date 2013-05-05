using StatCoach.Business.Interfaces;
using StatCoach.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace StatCoach.Business.Extensions
{
    public static class StringExtensions
    {
        public static string ToSEO(this string str)
        {
            return str.Replace(' ', '-');
        }

        public static string FromSEO(this string str)
        {
            return str.Replace('-', ' ');
        }
    }

    public static class HtmlHelpers
    {
        public static bool UserIsAuthorized(this HtmlHelper helper, IContent content)
        {
            if(!WebSecurity.IsAuthenticated)
                return false;

            using (UserRepository users = new UserRepository())
            {
                return users.UserIsAuthorized(content);
            }

            return false;
            
        }
    }
}
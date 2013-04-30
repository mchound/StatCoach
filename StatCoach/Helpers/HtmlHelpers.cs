using StatCoach.Data;
using StatCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}
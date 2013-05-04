using StatCoach.Business.Interfaces;
using StatCoach.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatCoach.Business
{
    public static class RequestData
    {
        private const string CLUB_KEY = "CLUB_ID";
        private const string CONTENT_KEY = "CONTENT";

        public static Club Club 
        {
            get
            {
                object obj = HttpContext.Current.Items[CLUB_KEY];
                if (obj != null)
                    return (Club)obj;
                return null;
            }
            set
            {
                HttpContext.Current.Items[CLUB_KEY] = value;
            }
        }

        public static IContent Content
        {
            get
            {
                object obj = HttpContext.Current.Items[CONTENT_KEY];
                if (obj != null)
                    return (IContent)obj;
                return null;
            }
            set
            {
                HttpContext.Current.Items[CONTENT_KEY] = value;
            }
        }
    }
}
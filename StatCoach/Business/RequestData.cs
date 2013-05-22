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
        private const string CLUB_KEY = "CLUB";
        private const string CONTENT_KEY = "CONTENT";
        private const string CLUB_ROUTE_NAME = "CLUB_ROUTE";
        private const string CONTENT_ROUTE_NAME = "CONTENT_ROUTE";

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

        public static string ClubRoute
        {
            get
            {
                object obj = HttpContext.Current.Items[CLUB_ROUTE_NAME];
                if (obj != null)
                    return (string)obj;
                return null;
            }
            set
            {
                HttpContext.Current.Items[CONTENT_KEY] = value;
            }
        }
    }

    public static class ReqData<T>
    {
        public static T Get(string key)
        {
            object obj = HttpContext.Current.Items[key];
            return (T)obj;
        }

        public static void Set(string key, T data)
        {
            HttpContext.Current.Items[key] = data;
        }
    }
}
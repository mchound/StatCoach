using StatCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
using System.Web.Mvc;

namespace StatCoach.Data
{
    public class StatsRepository : IDisposable
    {
        private dbStatCoachEntities db;
        private System.Web.Caching.Cache cache;

        public StatsRepository()
        {
            this.db = new dbStatCoachEntities();
            this.cache = new System.Web.Caching.Cache();
        }

        public IEnumerable<SelectListItem> GetClubListItems()
        {
            if (HttpContext.Current.Cache["ClucListItems"] == null)
            {
                HttpContext.Current.Cache["ClucListItems"] = this.db.Clubs.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).AsEnumerable<SelectListItem>();                
            }

            return (IEnumerable<SelectListItem>)HttpContext.Current.Cache["ClucListItems"];
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
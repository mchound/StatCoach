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
        private dbStatCoachEntitiesAzure db;
        private System.Web.Caching.Cache cache;

        public StatsRepository()
        {
            this.db = new dbStatCoachEntitiesAzure();
            this.cache = new System.Web.Caching.Cache();
        }

        public List<SelectListItem> GetClubListItems()
        {
            if (HttpContext.Current.Cache["ClucListItems"] == null)
            {
                HttpContext.Current.Cache["ClucListItems"] = this.db.Clubs.AsEnumerable<Club>().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList<SelectListItem>();                
            }

            return (List<SelectListItem>)HttpContext.Current.Cache["ClucListItems"];
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
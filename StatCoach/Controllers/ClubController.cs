using StatCoach.Data;
using StatCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StatCoach.Controllers
{
    public class ClubController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = this._club != null ? this._club.Name : "Ingen klubb hittad";
            
            List<TeamModel> model = new List<TeamModel>();

            if(this._club != null)
            {
                model = this._club.Teams.Select(t => new TeamModel
                {
                    Id = t.Id,
                    CreatedByUserId = t.CreatedByUserId,
                    Name = t.Name
                }).ToList();
            }
            
            return View(model);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatCoach.Business.Enums;
using StatCoach.Data;
using StatCoach.Models;
using StatCoach.Business;

namespace StatCoach.Controllers
{
    public class TeamController : BaseController
    {
        public ActionResult Index()
        {
            TeamModel model = this._content as TeamModel;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TeamModel model)
        {
            StatsRepository db = new StatsRepository();

            if (ModelState.IsValid)
            {
                CRUDStatus status = db.CreateTeam(model.Name);
                if (status != CRUDStatus.Success)
                    ModelState.AddModelError("", status.ToString());
                else
                    return RedirectToAction("Index", "Team");
            }

            return View(model);
        }

        public ActionResult Delete(string teamId)
        {
            if (!string.IsNullOrWhiteSpace(teamId))
            {
                if (this._statsRepository.DeleteTeam(teamId) == CRUDStatus.Success)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error when trying to delete team");
            }
            else
            {
                ModelState.AddModelError("", "No team selected");
            }

            object routeValues = new 
            { 
                club = ReqData<string>.Get("clubRoute"),
                content = ReqData<string>.Get("contentRoute"),
                controller = string.Empty,
                action = string.Empty
            };

            TeamModel model = this._content as TeamModel;
            return View("~/Views/Team/Index.cshtml", model);
        }

        private List<TeamModel> CreateModel()
        {
            StatsRepository db = new StatsRepository();
            return db.GetTeamsByCurrentUser();            
        }

    }
}

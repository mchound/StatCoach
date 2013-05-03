using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatCoach.Business.Enums;
using StatCoach.Data;
using StatCoach.Models;

namespace StatCoach.Controllers
{
    public class TeamController : Controller
    {
        public ActionResult Index()
        {
            List<TeamModel> model = this.CreateModel();
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
            StatsRepository db = new StatsRepository();
            if (!string.IsNullOrWhiteSpace(teamId))
            {
                if (db.DeleteTeam(teamId) == CRUDStatus.Success)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Error when trying to delete team");
            }
            else
            {
               ModelState.AddModelError("", "No team selected");
            }

            return View("~/Views/Team/Index.cshtml", this.CreateModel());
            
        }

        private List<TeamModel> CreateModel()
        {
            StatsRepository db = new StatsRepository();
            return db.GetTeamsByCurrentUser();            
        }

    }
}

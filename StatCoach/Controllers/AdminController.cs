using StatCoach.Data;
using StatCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatCoach.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View(new UsersModel());
        }

        public ActionResult Delete(int id)
        {
            using(UserRepository users = new UserRepository())
            {
                UserModel currentUser = users.GetCurrentUser();
                if (id == currentUser.Id)
                {
                    ModelState.AddModelError("UserLoggenOn", "Det går inte att ta bort en inloggad användare");
                    return View("~/Views/Admin/Index.cshtml", new UsersModel());
                }

                if (users.DeleteUser(id))
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("ServerError", "Det gick inte att ta bort användaren");
            return View("~/Views/Admin/Index.cshtml", new UsersModel());
            
        }

    }
}

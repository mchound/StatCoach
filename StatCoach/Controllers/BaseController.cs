using StatCoach.Business;
using StatCoach.Business.Interfaces;
using StatCoach.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatCoach.Controllers
{
    public class BaseController : Controller
    {
        protected Club _club;
        protected IContent _content;
        protected StatsRepository _statsRepository;

        public BaseController()
        {
            this._statsRepository = new StatsRepository();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this._club = RequestData.Club;
            this._content = RequestData.Content;

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this._statsRepository.Dispose();
            base.OnActionExecuted(filterContext);
        }

    }
}

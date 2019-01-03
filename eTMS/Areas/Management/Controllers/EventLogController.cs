using DevExpress.Data.Linq;
using DevExpress.Web.Mvc;
using eTMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eTMS.Areas.Management.Controllers
{
    [Authorize(Roles = "SU")]
    [RouteArea("management")]
    public class EventLogController : Controller
    {

        ILookUpRepo lookUpRepo;
        public EventLogController(ILookUpRepo _lookUpRepo)
        {
            lookUpRepo = _lookUpRepo;
        }

        // GET: Management/EventLog
        [Route("events")]
        public ActionResult EventLog()
        {
            ViewBag.Status = new SelectList(status().ToList());
            return View();
        }

        private IEnumerable<string> status()
        {
            string[] arry = { "All", "Today", "Yesterday", "Last two days", "Last three days" };
            return arry;
        }

        [ValidateInput(false)]
        public ActionResult EventLogGridViewPartial(string _status)
        {
            DateTime? date = null;
            switch (_status)
            {
                case "All":
                    date = date != null ? null : date;
                    break;
                case "Today":
                    date = DateTime.Now.Date;
                    break;
                case "Yesterday":
                    date = DateTime.Now.Date.AddDays(-1);
                    break;
                case "Last two days":
                    date = DateTime.Now.Date.AddDays(-2);
                    break;
                case "Last three days":
                    date = DateTime.Now.Date.AddDays(-3);
                    break;
            }
            return PartialView("_EventLogGridViewPartial", GetEntityDataSource(date));
        }

        private EntityServerModeSource GetEntityDataSource(DateTime? date)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "EventID";
            if (date != null)
            {
                DateTime d = date.Value;
                esms.QueryableSource = lookUpRepo.GetEventsByDate(d);
                return esms;
            }
            esms.QueryableSource = lookUpRepo.GetAllEvents();
            return esms;
        }
    }
}
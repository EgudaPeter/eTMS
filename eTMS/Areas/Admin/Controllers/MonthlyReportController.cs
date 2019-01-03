using DevExpress.Data.Linq;
using DevExpress.Web.Mvc;
using eTMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eTMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "SU,AC")]
    [RouteArea("admin")]
    public class MonthlyReportController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string currentMonth = null;
        #endregion

        IProfitAndLossRepo profitAndLossRepo;

        public MonthlyReportController(IProfitAndLossRepo _profitAndLossRepo)
        {
            profitAndLossRepo = _profitAndLossRepo;
        }

        // GET: Admin/MonthlyReport
        [Route("profitandloss/monthlyreport")]
        public ActionResult MonthlyReport()
        {
            ViewBag.Months = new SelectList(Months().ToList());
            return View();
        }

        private IEnumerable<string> Months()
        {
            string[] arry = new string[] {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };
            return arry.ToList();
        }


        [ValidateInput(false)]
        public ActionResult ProfitAndLoss_MonthlyGridViewPartial(string _month)
        {
            currentMonth = _month != null ? _month : currentMonth;
            return PartialView("_ProfitAndLoss_MonthlyGridViewPartial", GetEntityDataSource(_month));
        }

        private EntityServerModeSource GetEntityDataSource(string _month)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "PLID";
            if(_month != null)
            {
                esms.QueryableSource = profitAndLossRepo.GetAllProfitAndLossRecordsForAMonth(_month);
                return esms;
            }
            if(currentMonth != null)
            {
                esms.QueryableSource = profitAndLossRepo.GetAllProfitAndLossRecordsForAMonth(currentMonth);
                return esms;
            }
            esms.QueryableSource = null;
            return esms;
        }
    }
}
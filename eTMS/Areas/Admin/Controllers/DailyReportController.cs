using DevExpress.Data.Linq;
using DevExpress.Web.Mvc;
using eTMS.Common;
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
    public class DailyReportController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        IProfitAndLossRepo profitAndLossRepo;

        public DailyReportController(IProfitAndLossRepo _profitAndLossRepo)
        {
            profitAndLossRepo = _profitAndLossRepo;
        }

        // GET: Admin/DailyReport
        [Route("profitandloss/dailyreport")]
        public ActionResult DailyReport()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProfitAndLoss_DailyGridViewPartial(string _date)
        {
            return PartialView("_ProfitAndLoss_DailyGridViewPartial", GetEntityDataSource(_date));
        }

        private EntityServerModeSource GetEntityDataSource(string _date)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "PLID";
            if(_date != null)
            {
                string trimmedDate = Utilities.TrimDate(_date);
                DateTime capturedDate = Convert.ToDateTime(trimmedDate);
                esms.QueryableSource = profitAndLossRepo.GetAllProfitAndLossRecordsForDate(capturedDate);
                return esms;
            }
            esms.QueryableSource = null;
            return esms;
        }
    }
}
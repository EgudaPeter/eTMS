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
    public class WeeklyReportController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        #endregion

        IProfitAndLossRepo profitAndLossrepo;

        public WeeklyReportController(IProfitAndLossRepo _profitAndLossrepo)
        {
            profitAndLossrepo = _profitAndLossrepo;
        }

        // GET: Admin/WeeklyReport
        [Route("profitandloss/weeklyreport")]
        public ActionResult WeeklyReport()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProfitAndLoss_WeeklyGridViewPartial(string _sDate, string _eDate)
        {
            return PartialView("_ProfitAndLoss_WeeklyGridViewPartial", GetEntityDataSource(_sDate, _eDate));
        }

        private EntityServerModeSource GetEntityDataSource(string _sDate, string _eDate)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "PLID";
            if(_sDate != null && _eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_sDate);
                string trimmedEndDate = Utilities.TrimDate(_eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                esms.QueryableSource = profitAndLossrepo.GetAllProfitAndLossRecordsBetweenDates(startDate, endDate);
                return esms;
            }
            esms.QueryableSource = null;
            return esms;
        }

        public JsonResult CheckRangeOfEnteredDates(string _sDate, string _eDate)
        {
            try
            {
                string trimmedStartDate = Utilities.TrimDate(_sDate);
                string trimmedEndDate = Utilities.TrimDate(_eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                if(startDate.AddDays(7) == endDate)
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                return Json(new
                {
                    success = false,
                    infoMessage = $"The date range between the start date <b>({startDate.ToString("dd/MM/yyyy")})</b> and end date <b>({endDate.ToString("dd/MM/yyyy")})</b> is invalid! It has to be a week's difference!"
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = errorMessage
                });
            }
        }
    
    }
}
using eTMS.Common;
using eTMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eTMS.Areas.Home.Controllers
{
    [Authorize(Roles = "SU,AC,SA")]
    [RouteArea("home")]
    public class HomeController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        IProfitAndLossRepo plRepo;
        public HomeController(IProfitAndLossRepo _plRepo)
        {
            plRepo = _plRepo;
        }

        // GET: Home/Home
        [Route("etms")]
        public ActionResult Home()
        {
            try
            {
                var enableWeekendProfitAndLossAnalysis = plRepo.EnableWeekendProfitAndLossAnalysis();
                var thisDate = DateTime.Now.Date.ToString("dddd/MMMM/yyyy");
                var thisDay = thisDate.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (thisDay == "Monday" && enableWeekendProfitAndLossAnalysis == false)
                {
                    var lastFriday = DateTime.Now.Date.AddDays(-3);
                    plRepo.ComputeProfitAndLossForPreviousDay(lastFriday);
                    return View();
                }
                var yesterDay = DateTime.Now.Date.AddDays(-1);
                plRepo.ComputeProfitAndLossForPreviousDay(yesterDay);
                return View();
            }
            catch (Exception ex)
            {

                log.Error(ex.Message, ex);
                Settings.error = ex.Message;
            }
            return RedirectToAction("ApplicationError", "Error", new { Area = "Management" });
        }
    }
}
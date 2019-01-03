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
    public class SetupsController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        #endregion

        ISetupRepo setupRepo;

        public SetupsController(ISetupRepo _setupRepo)
        {
            setupRepo = _setupRepo;
        }

        // GET: Admin/Setups
        [Route("setups")]
        public ActionResult Setups()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveSettings( string _month, string enableCheckbox)
        {
            try
            {
                bool enableWeekendProfitAndLossAnalysis = enableCheckbox == null || enableCheckbox == "" ? false : true;
                setupRepo.SaveSettings(_month, enableWeekendProfitAndLossAnalysis);
                return Json(new
                {
                    success = true,
                    infoMessage = $"Settings saved successfully!"
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                var innerEx = ex.InnerException != null ? ex.InnerException.Message + " " + ex.InnerException.InnerException.Message : "";
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage}<br/> <br/> Error Details:{ex.Message} Inner exception details:{innerEx}"
                });
            }
        }

        public JsonResult GetValues()
        {
            try
            {
                var settings = setupRepo.GetSettings();
                return Json(new
                {
                    success = true,
                    month = settings.MonthOfOperation,
                    enableCheckbox = settings.EnableWeekendProfitAndLossAnalysis
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                var innerEx = ex.InnerException != null ? ex.InnerException.Message + " " + ex.InnerException.InnerException.Message : "";
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage}<br/> <br/> Error Details:{ex.Message} Inner exception details:{innerEx}"
                });
            }
        }
    }
}
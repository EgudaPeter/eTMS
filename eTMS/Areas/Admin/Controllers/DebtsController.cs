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
    public class DebtsController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string currentStatus = null;
        #endregion

        ILookUpRepo lookUpRepo;
        public DebtsController(ILookUpRepo _lookUpRepo)
        {
            lookUpRepo = _lookUpRepo;
        }

        // GET: Admin/Debts
        [Route("debts")]
        public ActionResult Debts()
        {
            ViewBag.Status = new SelectList(statusOptions().ToList());
            return View();
        }

        private IEnumerable<string> statusOptions()
        {
            string[] arry = new string[] { "All", "Pending", "Cleared"};
            return arry.ToList();
        }

        [ValidateInput(false)]
        public ActionResult DebtsGridViewPartial()
        {
            try
            {
                var statusName = Request.Form.GetValues("_status");
                if (statusName != null && statusName[0] != "")
                {
                    currentStatus = statusName[0];
                }
                return PartialView("_DebtsGridViewPartial", GetEntityDataSource(currentStatus));
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return PartialView("_DebtsGridViewPartial");
        }

        private EntityServerModeSource GetEntityDataSource(string status)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "DebtID";
            var model = from d in lookUpRepo.GetAllDebts()
                        join t in lookUpRepo.GetAllTransactions()
                        on d.TransactionID equals t.TransactionID
                        join f in lookUpRepo.GetAllFactors()
                        on t.FID equals f.FID
                        select new
                        {
                            DebtID = d.DebtID,
                            TransactionID = f.Name,
                            DeptName = d.DeptName,
                            EffectiveDate = d.EffectiveDate,
                            Amount = d.Amount,
                            Status = d.Status
                        };
            switch (status)
            {
                case null:
                    esms.QueryableSource = model;
                    break;
                case "All":
                    esms.QueryableSource = model.Where(x => x.Status == "P" || x.Status == "C");
                    break;
                case "Pending":
                    esms.QueryableSource = model.Where(x => x.Status == "P");
                    break;
                case "Cleared":
                    esms.QueryableSource = model.Where(x => x.Status == "C");
                    break;
            }
            return esms;
        }

        [HttpPost]
        public JsonResult ClearCurrentStatus()
        {
            currentStatus = currentStatus != null ? null : currentStatus;
            return Json(new { });
        }
    }
}
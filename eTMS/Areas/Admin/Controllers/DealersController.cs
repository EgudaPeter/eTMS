using DevExpress.Data.Linq;
using eTMS.BusinessObjectLayer;
using eTMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eTMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "SU,SA")]
    [RouteArea("admin")]
    public class DealersController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        static int tempCreatedID = -1;
        static int tempUpdatedID = -1;
        #endregion

        IDealersRepo dealerRepo;
        public DealersController(IDealersRepo _dealerRepo)
        {
            dealerRepo = _dealerRepo;
        }

        // GET: Admin/Dealers
        [Route("dealers")]
        public ActionResult Dealers()
        {
            ViewBag.Actions = new SelectList(actions().ToList());
            return View();
        }

        private IEnumerable<string> actions()
        {
            string[] arry = new string[] { "Select Action", "Delete" };
            return arry.ToList();
        }

        [ValidateInput(false)]
        public ActionResult DealerGridViewPartial()
        {
            if (tempUpdatedID > 0)
            {
                ViewBag.Key = tempUpdatedID;
                tempUpdatedID = -1;
            }
            if (tempCreatedID > 0)
            {
                ViewBag.Key = tempCreatedID;
                tempCreatedID = -1;
            }
            return PartialView("_DealerGridViewPartial", GetEntityDataSource());
        }

        private EntityServerModeSource GetEntityDataSource()
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "DealerID";
            esms.QueryableSource = dealerRepo.GetAllDealers();
            return esms;
        }

        #region Functions
        [HttpPost]
        public JsonResult AddDealer(DealerTable Data)
        {
            try
            {
                Data.CapturedBy = User.Identity.Name;
                Data.CapturedDate = DateTime.Now.Date;
                tempCreatedID = dealerRepo.AddDealer(Data);
                return Json(new
                {
                    success = true,
                    infoMessage = "Dealer added successfully!"
                });

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult EditDealer(string dealerId)
        {
            try
            {
                int ID = int.Parse(dealerId);
                var record = dealerRepo.FindDealer(ID);
                return Json(new
                {
                    success = true,
                    dealerID = record.DealerID,
                    dealerName = record.DealerName
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult UpdateDealer(DealerTable Data)
        {
            try
            {
                Data.CapturedBy = User.Identity.Name; // used to get the name of the modifier
                tempUpdatedID = dealerRepo.UpdateDealer(Data);
                return Json(new
                {
                    success = true,
                    infoMessage = "Dealer updated successfully!"
                });

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult ValidateUniquenessOfDealerName(string dName)
        {
            try
            {
                var result = dealerRepo.CheckIfDealerNameIsUnique(dName);
                if (result == true)
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                return Json(new
                {
                    success = false,
                    infoMessage = "Dealer name already exists!"
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult DeleteGroupOfDealers(string selectedKeys)
        {
            try
            {
                string[] DealerIDs = selectedKeys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                dealerRepo.DeleteGroupOfDealers(DealerIDs);
                return Json(new
                {
                    success = true,
                    infoMessage = "Dealer(s) successfully deleted!"
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult DeleteASingleDealer(string selectedKey)
        {
            try
            {
                int ID = int.Parse(selectedKey);
                dealerRepo.DeleteASingleDealer(ID);
                return Json(new
                {
                    success = true,
                    infoMessage = "Dealer successfully deleted!"
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }
        #endregion
    }
}
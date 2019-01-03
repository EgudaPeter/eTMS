using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTMS.BusinessObjectLayer;
using DevExpress.Data.Linq;
using eTMS.Repositories.Interfaces;
using System.Data.Entity.Validation;

namespace eTMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "SU,SA")]
    [RouteArea("admin")]
    public class FactorController : Controller
    {
        IFactorRepo factorRepo;
        public FactorController(IFactorRepo _factorRepo)
        {
            factorRepo = _factorRepo;
        }

        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        static int tempCreatedID = -1;
        static int tempUpdatedID = -1;
        #endregion

        // GET: Admin/Expense
        [Route("factors")]
        public ActionResult Factor()
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
        public ActionResult FactorGridViewPartial()
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
            return PartialView("_FactorGridViewPartial", GetEntityDataSource());
        }

        private EntityServerModeSource GetEntityDataSource()
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "FID";
            esms.QueryableSource = factorRepo.GetAllFactors();
            return esms;
        }

        #region Functions
        [HttpPost]
        public JsonResult AddFactor(TransactionFactorTable Data)
        {
            var exceptionMessage = string.Empty;
            try
            {
                Data.CapturedBy = User.Identity.Name;
                Data.CapturedDate = DateTime.Now.Date;
                tempCreatedID = factorRepo.AddFactor(Data);
                return Json(new
                {
                    success = true,
                    infoMessage = "Factor added successfully!"
                });

            }
            catch (DbEntityValidationException dbEx)
            {
                var err = dbEx.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var fullErr = string.Join(";", err);

                exceptionMessage = string.Concat(dbEx.Message, $"Exact Message {fullErr}");
            }

            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                exceptionMessage = ex.Message + ex.InnerException != null ? ex.InnerException.Message + " " + ex.InnerException.InnerException.Message : "";
            }
            return Json(new
            {
                success = -1,
                errorMessage = $"{errorMessage} Error Detail:{exceptionMessage}"
            });
        }

        [HttpPost]
        public JsonResult EditFactor(string factorId)
        {
            try
            {
                int ID = int.Parse(factorId);
                var record = factorRepo.FindFactor(ID);
                return Json(new
                {
                    success = true,
                    FID = record.FID,
                    factorName = record.Name
                });
            }
            catch(Exception ex)
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
        public JsonResult UpdateFactor(TransactionFactorTable Data)
        {
            var exceptionMessage = string.Empty;
            try
            {
                Data.CapturedBy = User.Identity.Name; //used to get the name of the modifier
                tempUpdatedID = factorRepo.UpdateFactor(Data);
                return Json(new
                {
                    success = true,
                    infoMessage = "Factor updated successfully!"
                });

            }
            catch (DbEntityValidationException dbEx)
            {
                var err = dbEx.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var fullErr = string.Join(";", err);

                exceptionMessage = string.Concat(dbEx.Message, $"Exact Message {fullErr}");
            }

            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                exceptionMessage = ex.Message + ex.InnerException != null ? ex.InnerException.Message + " " + ex.InnerException.InnerException.Message : "";
            }
            return Json(new
            {
                success = -1,
                errorMessage = $"{errorMessage} Error Detail:{exceptionMessage}"
            });
        }

        [HttpPost]
        public JsonResult ValidateUniquenessOfExpenseName(string fName)
        {
            try
            {
                var result = factorRepo.CheckIfFactorNameIsUnique(fName);
                if(result == true)
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                return Json(new
                {
                    success = false,
                    infoMessage = "Factor name already exists!"
                });
            }
            catch(Exception ex)
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
        public JsonResult DeleteGroupOfFactors(string selectedKeys)
        {
            try
            {
                string[] factorIDs = selectedKeys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                factorRepo.DeleteGroupOfFactors(factorIDs);
                return Json(new
                {
                    success = true,
                    infoMessage = "Factor(s) successfully deleted!"
                });
            }
            catch(Exception ex)
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
        public JsonResult DeleteASingleFactor(string selectedKey)
        {
            try
            {
                int ID = int.Parse(selectedKey);
                factorRepo.DeleteASingleFactor(ID);
                return Json(new
                {
                    success = true,
                    infoMessage = "Factor successfully deleted!"
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
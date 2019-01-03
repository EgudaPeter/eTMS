using DevExpress.Data.Linq;
using DevExpress.Web.Mvc;
using eTMS.BusinessObjectLayer;
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
    public class AccountsController : Controller
    {

        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        static int tempCreatedID = -1;
        static int tempUpdatedID = -1;
        #endregion

        IAccountRepo accountRepo;
        public AccountsController(IAccountRepo _accountRepo)
        {
            accountRepo = _accountRepo;
        }

        // GET: Admin/Accounts
        [Route("accounts")]
        public ActionResult Accounts()
        {
            ViewBag.Actions = new SelectList(actions().ToList());
            ViewBag.Banks = accountRepo.GetAllBanks().ToList();
            return View();
        }

        private IEnumerable<string> actions()
        {
            string[] arry = new string[] { "Select Action", "Delete" };
            return arry.ToList();
        }

        [ValidateInput(false)]
        public ActionResult AccountGridViewPartial()
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

            return PartialView("_AccountGridViewPartial", GetEntityDataSource());
        }

        private EntityServerModeSource GetEntityDataSource()
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "AccountID";
            var model = from ac in accountRepo.GetAllAccounts()
                        join ba in accountRepo.GetAllBanks()
                        on ac.BankName equals ba.ID_Bank
                        select new
                        {
                            AccountID = ac.AccountID,
                            AccountNumber = ac.AccountNumber,
                            AccountName = ac.AccountName,
                            BankName = ba.BankName,
                            CapturedBy = ac.CapturedBy,
                            CapturedDate = ac.CapturedDate,
                            AmountInAccount = ac.AmountInAccount
                        };
            esms.QueryableSource = model;
            return esms;
        }

        #region Functions
        [HttpPost]
        public JsonResult AddAccount(AccountTable Data)
        {
            try
            {
                Data.CapturedBy = User.Identity.Name;
                Data.CapturedDate = DateTime.Now.Date;
                tempCreatedID = accountRepo.AddAccount(Data);
                return Json(new
                {
                    success = true,
                    infoMessage = "Account added successfully!"
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
        public JsonResult EditAccount(string accountId)
        {
            try
            {
                int ID = int.Parse(accountId);
                var record = accountRepo.FindAccount(ID);
                return Json(new
                {
                    success = true,
                    accountID = record.AccountID,
                    accountNumber = record.AccountNumber,
                    accountName = record.AccountName,
                    bankID = record.BankName,
                    amount = record.AmountInAccount
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
        public JsonResult ViewAccount(string accountId)
        {
            try
            {
                int ID = int.Parse(accountId);
                var record = accountRepo.FindAccount(ID);
                return Json(new
                {
                    success = true,
                    accountID = record.AccountID,
                    accountNumber = record.AccountNumber,
                    accountName = record.AccountName,
                    bankID = record.BankName,
                    amount = record.AmountInAccount
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
        public JsonResult UpdateAccount(AccountTable Data)
        {
            try
            {
                Data.CapturedBy = User.Identity.Name; //used to get the name of the modifier
                tempUpdatedID = accountRepo.UpdateAccount(Data);
                return Json(new
                {
                    success = true,
                    infoMessage = "Account updated successfully!"
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
        public JsonResult ValidateUniquenessOfAccountNumber(string aNumber)
        {
            try
            {
                var result = accountRepo.CheckIfAccountNumberIsUnique(aNumber);
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
                    infoMessage = "Account number already exists!"
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
        public JsonResult DeleteGroupOfAccounts(string selectedKeys)
        {
            try
            {
                string[] accountIDs = selectedKeys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                accountRepo.DeleteGroupOfAccounts(accountIDs);
                return Json(new
                {
                    success = true,
                    infoMessage = "Account(s) successfully deleted!"
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
        public JsonResult DeleteASingleAccount(string selectedKey)
        {
            try
            {
                int ID = int.Parse(selectedKey);
                accountRepo.DeleteASingleAccount(ID);
                return Json(new
                {
                    success = true,
                    infoMessage = "Account successfully deleted!"
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
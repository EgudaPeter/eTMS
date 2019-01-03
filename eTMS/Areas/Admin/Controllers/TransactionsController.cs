using DevExpress.Data.Linq;
using DevExpress.Web.Mvc;
using eTMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eTMS.BusinessObjectLayer.Models;
using eTMS.Common;

namespace eTMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "SU,AC,SA")]
    [RouteArea("admin")]
    public class TransactionsController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        static int tempCreatedID = -1;
        static int tempUpdatedID = -1;
        static string currentStatus = null;
        #endregion

        ITransactionRepo transRepo;
        IFactorRepo factorRepo;
        IAccountRepo accountRepo;
        IDealersRepo dealerRepo;
        ILookUpRepo lookUpRepo;
        public TransactionsController(ITransactionRepo _transRepo, IFactorRepo _factorRepo,
            IAccountRepo _accountRepo, IDealersRepo _dealerRepo, ILookUpRepo _lookUpRepo)
        {
            transRepo = _transRepo; factorRepo = _factorRepo;
            accountRepo = _accountRepo; dealerRepo = _dealerRepo; lookUpRepo = _lookUpRepo;
        }

        // GET: Admin/Transactions
        [Route("transactions")]
        public ActionResult Transactions()
        {
            ViewBag.Status = new SelectList(statusOptions().ToList());
            ViewBag.Actions = new SelectList(actions().ToList());

            ViewBag.TransactionTypes = transRepo.GetAllTransactionTypes().ToList();
            ViewBag.Factors = factorRepo.GetAllFactors().ToList(); ;
            ViewBag.Accounts = accountRepo.GetAllAccounts().ToList(); ;
            ViewBag.Dealers = dealerRepo.GetAllDealers().ToList(); ;

            return View();
        }

        private IEnumerable<string> actions()
        {
            string[] arry = new string[] { "Select Action", "Delete" };
            return arry.ToList();
        }

        private IEnumerable<string> statusOptions()
        {
            string[] arry = new string[] { "All", "Expense", "Income", "Cleared", "Outstanding" };
            return arry.ToList();
        }

        [ValidateInput(false)]
        public ActionResult TransactionGridViewPartial()
        {
            try
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
                var statusName = Request.Form.GetValues("_status");
                if(statusName != null && statusName[0] != "")
                {
                    currentStatus = statusName[0];
                }
                return PartialView("_TransactionGridViewPartial", GetEntityDataSource(currentStatus));
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return PartialView("_TransactionGridViewPartial");
        }

        private EntityServerModeSource GetEntityDataSource(string status)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "TransactionID";
            var model = from tr in lookUpRepo.GetAllTransactions()
                        join t in lookUpRepo.GetAllTransactionTypes()
                        on tr.TID equals t.TID
                        join f in lookUpRepo.GetAllFactors()
                        on tr.FID equals f.FID
                        join d in lookUpRepo.GetAllDealers()
                        on tr.DealerID equals d.DealerID
                        join ac in lookUpRepo.GetAllAccounts()
                        on tr.AccountID equals ac.AccountID
                        select new
                        {
                            TransactionID = tr.TransactionID,
                            TID = t.Type,
                            FID = f.Name,
                            AccountID = ac.AccountNumber,
                            Bank = tr.Bank,
                            DealerID = d.DealerName,
                            EffectiveDate = tr.EffectiveDate,
                            CapturedBy = tr.CapturedBy,
                            PaymentMode = tr.PaymentMode,
                            Narration = tr.Narration,
                            AmountPaid = tr.AmountPaid,
                            AmountOutstanding = tr.AmountOutstanding
                        };
            switch (status)
            {
                case null:
                    esms.QueryableSource = model;
                    break;
                case "All":
                    esms.QueryableSource = model.Where(x => x.TID == "Expense" || x.TID == "Income");
                    break;
                case "Expense":
                    esms.QueryableSource = model.Where(x=>x.TID == "Expense");
                    break;
                case "Income":
                    esms.QueryableSource = model.Where(x => x.TID == "Income");
                    break;
                case "Cleared":
                    esms.QueryableSource = model.Where(x => x.AmountOutstanding == 0.00m);
                    break;
                case "Outstanding":
                    esms.QueryableSource = model.Where(x => x.AmountOutstanding > 0.00m);
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

        #region Functions
        [HttpPost]
        public JsonResult GetAmountInBank(string accountID)
        {
            try
            {
                int ID = int.Parse(accountID);
                decimal amount = transRepo.GetAmountInBank(ID);
                return Json(new { success = true, Amount = amount });
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

        [HttpPost]
        public JsonResult SaveTransaction(TransactionModel Data)
        {
            try
            {
                string effectiveDate = Data.EffectiveDateString == null ? null : Utilities.TrimDate(Data.EffectiveDateString);
                Data.EffectiveDate = effectiveDate != null ? Convert.ToDateTime(effectiveDate) : Data.EffectiveDate;
                Data.CapturedBy = User.Identity.Name;
                Data.CapturedDate = DateTime.Now.Date;
                tempCreatedID = transRepo.AddTransaction(Data);
                if(tempCreatedID != -1)
                {
                    return Json(new
                    {
                        success = true,
                        infoMessage = "Transaction successfully saved!"
                    });
                }
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage}<br/> <br/> Error Details: SaveTransaction method returned a negative value."
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                if(tempCreatedID != -1 && tempCreatedID > 0)
                {
                    transRepo.ReverseAction(tempCreatedID);
                }
                var innerEx = ex.InnerException != null ? ex.InnerException.Message + " " + ex.InnerException.InnerException.Message : "";
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage}<br/> <br/> Error Details:{ex.Message} Inner exception details:{innerEx}"
                });
            }
        }

        [HttpPost]
        public JsonResult UpdateTransaction(TransactionModel Data)
        {
            try
            {
                string effectiveDate = Data.EffectiveDateString == null ? null : Utilities.TrimDate(Data.EffectiveDateString);
                Data.EffectiveDate = effectiveDate != null ? Convert.ToDateTime(effectiveDate) : Data.EffectiveDate;
                Data.CapturedBy = User.Identity.Name; //used to get the name of the modifier
                tempUpdatedID = transRepo.UpdateTransaction(Data);
                if(tempUpdatedID != -1)
                {
                    return Json(new
                    {
                        success = true,
                        infoMessage = "Transaction successfully updated!"
                    });
                }
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage}<br/> <br/> Error Details: UpdateTransaction method returned a negative value."
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

        [HttpPost]
        public JsonResult ViewTransaction(string transactionID)
        {
            try
            {
                int ID = int.Parse(transactionID);
                var transactionRecord = transRepo.FindTransaction(ID);
                return Json(new
                {
                    success = true,
                    transactionID = transactionRecord.TransactionID,
                    recordType = transactionRecord.TID,
                    fID = transactionRecord.FID,
                    amountPaid = transactionRecord.AmountPaid,
                    amountOutstanding = transactionRecord.AmountOutstanding,
                    effectiveDate = transactionRecord.EffectiveDate == null ? null : transactionRecord.EffectiveDate.Value.Date.ToString("MM/dd/yyyy"),
                    account = transactionRecord.AccountID,
                    dealer = transactionRecord.DealerID,
                    paymentMode = transactionRecord.PaymentMode,
                    narration = transactionRecord.Narration
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

        [HttpPost]
        public JsonResult EditTransaction(string transactionID)
        {
            try
            {
                int ID = int.Parse(transactionID);
                var transactionRecord = transRepo.FindTransaction(ID);
                return Json(new
                {
                    success = true,
                    transactionID = transactionRecord.TransactionID,
                    recordType = transactionRecord.TID,
                    fID = transactionRecord.FID,
                    amountPaid = transactionRecord.AmountPaid,
                    amountOutstanding = transactionRecord.AmountOutstanding,
                    effectiveDate = transactionRecord.EffectiveDate == null ? null : transactionRecord.EffectiveDate.Value.Date.ToString("MM/dd/yyyy"),
                    account = transactionRecord.AccountID,
                    dealer = transactionRecord.DealerID,
                    paymentMode = transactionRecord.PaymentMode,
                    narration = transactionRecord.Narration
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

        [HttpPost]
        public JsonResult DeleteASingleTransaction(string selectedKey)
        {
            try
            {
                int transactionID = int.Parse(selectedKey);
                transRepo.DeleteASingleTransaction(transactionID, User.Identity.Name);
                return Json(new
                {
                    success = true,
                    nfoMessage = "Transaction successfully deleted!"
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                var innerEx = ex.InnerException != null? ex.InnerException.Message + " " + ex.InnerException.InnerException.Message :"";
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage}<br/> <br/> Error Details:{ex.Message} Inner exception details:{innerEx}"
                });
            }
        }

        [HttpPost]
        public JsonResult DeleteAGroupOfTransactions(string selectedKeys)
        {
            try
            {
                string[] transactionIDs = selectedKeys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                transRepo.DeleteGroupOfTransactions(transactionIDs, User.Identity.Name);
                return Json(new
                {
                    success = true,
                    infoMessage = "Transaction(s) successfully deleted!"
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
        #endregion
    }
}
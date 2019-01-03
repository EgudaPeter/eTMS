using DevExpress.Web.Mvc;
using System;
using System.Web.Mvc;
using System.Linq;
using eTMS.Repositories.Interfaces;
using eTMS.Common;
using DevExpress.Data.Linq;

namespace eTMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "SU,AC,SA")]
    [RouteArea("admin")]
    public class AdminController : Controller
    {

        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        static string sDate = null;
        static string eDate = null;
        #endregion

        ILookUpRepo lookUpRepo;
        IAccountRepo accountRepo;
        IDealersRepo dealerRepo;
        public AdminController(ILookUpRepo _lookUpRepo, IAccountRepo _accountRepo, IDealersRepo _dealerRepo)
        {
            lookUpRepo = _lookUpRepo; accountRepo = _accountRepo;
            dealerRepo = _dealerRepo;
        }

        [Route("dashboard")]
        public ActionResult Dashboard()
        {
            try
            {
                ViewBag.Transactions = lookUpRepo.GetAllTransactions().Count(x => x.AmountOutstanding == 0.00m || x.AmountOutstanding > 0.00m);
                ViewBag.Transactions_Amount = Utilities.FormatMoney(lookUpRepo.GetTotalAmountForTransactions(), 2);

                ViewBag.Receipts = lookUpRepo.GetAllTransactions().Count(x => x.AmountOutstanding == 0.00m);
                ViewBag.Receipts_Amount = Utilities.FormatMoney(lookUpRepo.GetTotalAmountForReceipts(), 2);

                ViewBag.Accounts = lookUpRepo.GetAllAccounts().Count();
                ViewBag.Accounts_Amount = Utilities.FormatMoney(lookUpRepo.GetTotalAmountForAccounts(), 2);

                ViewBag.Debtors = lookUpRepo.GetAllDebtors().Count(x => x.Status == "P");
                ViewBag.Debtors_Amount = Utilities.FormatMoney(lookUpRepo.GetTotalAmountForDebtors(), 2);

                ViewBag.ResolvedDebtors = lookUpRepo.GetAllDebtors().Count(x => x.Status == "C");
                ViewBag.ResolvedDebtors_Amount = Utilities.FormatMoney(lookUpRepo.GetTotalAmountForResolvedDebtors(), 2);

                ViewBag.Dealers = lookUpRepo.GetAllDealers().Count();

                ViewBag.Debts = lookUpRepo.GetAllDebts().Count(x => x.Status == "P");
                ViewBag.Debts_Amount = Utilities.FormatMoney(lookUpRepo.GetTotalAmountForDebts(), 2);

                ViewBag.ResolvedDebts = lookUpRepo.GetAllDebts().Count(x => x.Status == "C");
                ViewBag.ResolvedDebts_Amount = Utilities.FormatMoney(lookUpRepo.GetTotalAmountForResolvedDebts(), 2);

                ViewBag.Profit = Utilities.FormatMoney(lookUpRepo.GetTotalProfit(), 2);
                ViewBag.Loss = Utilities.FormatMoney(lookUpRepo.GetTotalLoss(), 2);
                return View();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                Settings.error = ex.Message;
            }
            return RedirectToAction("ApplicationError", "Error", new { Area = "Management" });
        }

        public JsonResult RefreshScreen(string _startDate, string _endDate)
        {
            try
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var transactionsModel = lookUpRepo.GetTransactionsBetweenDates(startDate, endDate);
                var accountsModel = lookUpRepo.GetAccountsBetweenDates(startDate, endDate);
                var debtsModel = lookUpRepo.GetDebtsBetweenDates(startDate, endDate);
                var debtorsModel = lookUpRepo.GetDebtorsBetweenDates(startDate, endDate);
                var dealersModel = lookUpRepo.GetDealersBetweenDates(startDate, endDate);
                var profitAndLossModel = lookUpRepo.GetProfitAndLossRecordsBetweenDates(startDate, endDate);

                int transactionsCount = 0; decimal transactions_amount = 0.00m;
                int receiptsCount = 0; decimal receipts_amount = 0.00m;
                int accountsCount = 0; decimal accounts_amount = 0.00m;
                int debtorsCount = 0; decimal debtors_amount = 0.00m;
                int resolvedDebtorsCount = 0; decimal resolvedDebtors_amount = 0.00m;
                int dealersCount = 0;
                int debtsCount = 0; decimal debts_amount = 0.00m;
                int resolvedDebtsCount = 0; decimal resolvedDebts_amount = 0.00m;
                int profitAndLossCount = 0; decimal profit = 0.00m; decimal loss = 0.00m;

                #region Transactions
                transactionsCount = transactionsModel.Count(x => x.AmountOutstanding == 0.00m || x.AmountOutstanding > 0.00m);
                if (transactionsCount > 0)
                {
                    transactions_amount = transactionsModel.Sum(x => x.AmountPaid);
                }

                receiptsCount = transactionsModel.Count(x => x.AmountOutstanding == 0.00m);
                var receiptsModel = transactionsModel.Where(x => x.AmountOutstanding == 0.00m);
                if (receiptsCount > 0)
                {
                    receipts_amount = receiptsModel.Sum(x => x.AmountPaid);
                }
                #endregion

                #region Accounts
                accountsCount = accountsModel.Count();
                if (accountsCount > 0)
                {
                    accounts_amount = accountsModel.Sum(x => x.AmountInAccount);
                }
                #endregion

                #region Debtors
                debtorsCount = debtorsModel.Count(x => x.Status == "P");
                var debtors = debtorsModel.Where(x => x.Status == "P");
                if (debtorsCount > 0)
                {
                    debtors_amount = debtors.Sum(x => x.Amount);
                }

                resolvedDebtorsCount = debtorsModel.Count(x => x.Status == "C");
                var resolvedDebtors = debtorsModel.Where(x => x.Status == "C");
                if (resolvedDebtorsCount > 0)
                {
                    resolvedDebtors_amount = resolvedDebtors.Sum(x => x.Amount);
                }
                #endregion

                #region Dealers
                dealersCount = dealersModel.Count();
                #endregion

                #region Debts
                debtsCount = debtsModel.Count(x => x.Status == "P");
                var debts = debtsModel.Where(x => x.Status == "P");
                if (debtsCount > 0)
                {
                    debts_amount = debts.Sum(x => x.Amount);
                }

                resolvedDebtsCount = debtsModel.Count(x => x.Status == "C");
                var resolvedDebts = debtsModel.Where(x => x.Status == "C");
                if (resolvedDebtsCount > 0)
                {
                    resolvedDebts_amount = resolvedDebts.Sum(x => x.Amount);
                }
                #endregion

                #region Profit And Loss
                profitAndLossCount = profitAndLossModel.Count();
                var profitAndLossRecords = profitAndLossModel;
                if(profitAndLossCount > 0)
                {
                    profit = profitAndLossRecords.Sum(x => x.Profit);
                    loss = profitAndLossRecords.Sum(x => x.Loss);
                }
                #endregion

                return Json(new
                {
                    success = true,
                    transactions = transactionsCount,
                    transactions_amount = Utilities.FormatMoney(transactions_amount, 2),
                    receipts = receiptsCount,
                    receipts_amount = Utilities.FormatMoney(receipts_amount, 2),
                    accounts = accountsCount,
                    accounts_amount = Utilities.FormatMoney(accounts_amount, 2),
                    debtors = debtorsCount,
                    debtors_amount = Utilities.FormatMoney(debtors_amount, 2),
                    resolvedDebtors = resolvedDebtorsCount,
                    resolvedDebtors_amount = Utilities.FormatMoney(resolvedDebtors_amount, 2),
                    dealers = dealersCount,
                    debts = debtsCount,
                    debts_amount = Utilities.FormatMoney(debts_amount, 2),
                    resolvedDebts = resolvedDebtsCount,
                    resolvedDebts_amount = Utilities.FormatMoney(resolvedDebts_amount, 2),
                    profit = Utilities.FormatMoney(profit,2),
                    loss = Utilities.FormatMoney(loss,2)
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

        #region Dashboard_Transactions
        [ValidateInput(false)]
        public ActionResult DashBoard_TransactionsGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_TransactionsGridViewPartial", DashBoardTransactions_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashBoardTransactions_GetEntityDataSource(string _startDate, string _endDate)
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
                            CapturedDate = tr.CapturedDate,
                            AmountPaid = tr.AmountPaid,
                            AmountOutstanding = tr.AmountOutstanding
                        };
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            esms.QueryableSource = model;
            return esms;
        }
        #endregion

        #region Dashboard_Receipts
        [ValidateInput(false)]
        public ActionResult DashBoard_ReceiptsGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_ReceiptsGridViewPartial", DashBoardReceipts_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashBoardReceipts_GetEntityDataSource(string _startDate, string _endDate)
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
                            CapturedDate = tr.CapturedDate,
                            AmountPaid = tr.AmountPaid,
                            AmountOutstanding = tr.AmountOutstanding
                        };
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
                esms.QueryableSource = filteredModel.Where(x => x.AmountOutstanding == 0.00m);
                return esms;
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            esms.QueryableSource = model.Where(x => x.AmountOutstanding == 0.00m);
            return esms;
        }
        #endregion

        #region Dashboard_Accounts
        [ValidateInput(false)]
        public ActionResult DashBoard_AccountsGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_AccountsGridViewPartial", DashboardAccounts_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashboardAccounts_GetEntityDataSource(string _startDate, string _endDate)
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
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                esms.QueryableSource = model.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
                return esms;
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            esms.QueryableSource = model;
            return esms;
        }
        #endregion

        #region Dashboard_Debtors
        [ValidateInput(false)]
        public ActionResult DashBoard_DebtorsGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_DebtorsGridViewPartial", DashboardDebtors_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashboardDebtors_GetEntityDataSource(string _startDate, string _endDate)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "DebtorID";
            var model = from d in lookUpRepo.GetAllDebtors()
                        join t in lookUpRepo.GetAllTransactions()
                        on d.TransactionID equals t.TransactionID
                        join f in lookUpRepo.GetAllFactors()
                        on t.FID equals f.FID
                        select new
                        {
                            DebtorID = d.DebtorID,
                            TransactionID = f.Name,
                            DeptorName = d.DeptorName,
                            EffectiveDate = d.EffectiveDate,
                            Amount = d.Amount,
                            Status = d.Status
                        };
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
                esms.QueryableSource = filteredModel.Where(x => x.Status == "P");
                return esms;
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            esms.QueryableSource = model.Where(x => x.Status == "P");
            return esms;
        }
        #endregion

        #region Dashboard_ResolvedDebtors
        [ValidateInput(false)]
        public ActionResult DashBoard_ResolvedDebtorsGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_ResolvedDebtorsGridViewPartial", DashboardResolvedDebtors_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashboardResolvedDebtors_GetEntityDataSource(string _startDate, string _endDate)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "DebtorID";
            var model = from d in lookUpRepo.GetAllDebtors()
                        join t in lookUpRepo.GetAllTransactions()
                        on d.TransactionID equals t.TransactionID
                        join f in lookUpRepo.GetAllFactors()
                        on t.FID equals f.FID
                        select new
                        {
                            DebtorID = d.DebtorID,
                            TransactionID = f.Name,
                            DeptorName = d.DeptorName,
                            EffectiveDate = d.EffectiveDate,
                            Amount = d.Amount,
                            Status = d.Status
                        };
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
                esms.QueryableSource = filteredModel.Where(x => x.Status == "C");
                return esms;
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            esms.QueryableSource = model.Where(x => x.Status == "C");
            return esms;
        }
        #endregion

        #region Dashboard_Dealers
        [ValidateInput(false)]
        public ActionResult DashBoard_DealersGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_DealersGridViewPartial", DashboardDealers_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashboardDealers_GetEntityDataSource(string _startDate, string _endDate)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "DealerID";
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                esms.QueryableSource = lookUpRepo.GetDealersBetweenDates(startDate, endDate);
                return esms;
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = lookUpRepo.GetDealersBetweenDates(startDate, endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            esms.QueryableSource = dealerRepo.GetAllDealers();
            return esms;
        }
        #endregion

        #region Dashboard_Debts
        [ValidateInput(false)]
        public ActionResult DashBoard_DebtsGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_DebtsGridViewPartial", DashboardDebts_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashboardDebts_GetEntityDataSource(string _startDate, string _endDate)
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
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
                esms.QueryableSource = filteredModel.Where(x => x.Status == "P");
                return esms;
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            esms.QueryableSource = model.Where(x => x.Status == "P");
            return esms;
        }
        #endregion

        #region Dashboard_ResolvedDebts
        [ValidateInput(false)]
        public ActionResult DashBoard_ResolvedDebtsGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_ResolvedDebtsGridViewPartial", DashboardResolvedDebts_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashboardResolvedDebts_GetEntityDataSource(string _startDate, string _endDate)
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
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
                esms.QueryableSource = filteredModel.Where(x => x.Status == "C");
                return esms;
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                var filteredModel = model.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
                esms.QueryableSource = filteredModel;
                return esms;
            }
            esms.QueryableSource = model.Where(x => x.Status == "C");
            return esms;
        }
        #endregion

        #region Dashboard_ProfitAndLoss
        [ValidateInput(false)]
        public ActionResult DashBoard_ProfitAndLossGridViewPartial(string _startDate, string _endDate)
        {
            sDate = _startDate != null ? _startDate : sDate; eDate = _endDate != null ? _endDate : eDate;
            return PartialView("_DashBoard_ProfitAndLossGridViewPartial", DashboardProfitAndLoss_GetEntityDataSource(_startDate, _endDate));
        }

        private EntityServerModeSource DashboardProfitAndLoss_GetEntityDataSource(string _startDate, string _endDate)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "PLID";
            esms.QueryableSource = lookUpRepo.GetAllProfitAndLossRecords();
            if (_startDate != null && _endDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(_startDate);
                string trimmedEndDate = Utilities.TrimDate(_endDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                esms.QueryableSource = lookUpRepo.GetProfitAndLossRecordsBetweenDates(startDate, endDate);
            }
            if (sDate != null && eDate != null)
            {
                string trimmedStartDate = Utilities.TrimDate(sDate);
                string trimmedEndDate = Utilities.TrimDate(eDate);
                DateTime startDate = Convert.ToDateTime(trimmedStartDate);
                DateTime endDate = Convert.ToDateTime(trimmedEndDate);
                esms.QueryableSource = lookUpRepo.GetProfitAndLossRecordsBetweenDates(startDate, endDate);
            }
            return esms;
        }
        #endregion

        public JsonResult ClearStaticDateFields()
        {
            try
            {
                sDate = sDate != null ? null : sDate;
                eDate = eDate != null ? null : eDate;
                return Json(new
                {
                    success = true
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
    }
}
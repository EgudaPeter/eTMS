using eTMS.BusinessObjectLayer;
using eTMS.Repositories.Interfaces;
using System;
using System.Linq;
using eTMS.BusinessObjectLayer.Models;

namespace eTMS.Repositories.Repositories
{
    public class TransactionRepo : BaseRepository<eTMSEntities>, ITransactionRepo
    {
        public void AddDebt(Debt debt)
        {
            DataContext.Debts.Add(debt);
            SaveAll();
        }

        public void AddDebtor(Debtor debtor)
        {
            DataContext.Debtors.Add(debtor);
            SaveAll();
        }

        public int AddTransaction(TransactionModel model)
        {
            decimal totalAmount = 0.00m;

            var account = DataContext.AccountTables.Where(x => x.AccountID == model.AccountID).FirstOrDefault();
            var bankName = DataContext.Banks.Where(x => x.ID_Bank == account.BankName).FirstOrDefault().BankName;

            #region For Expense Transactions
            if (model.TID == "EX")
            {
                model.CapturedBy = model.CapturedBy == null ? "" : model.CapturedBy;
                model.PaymentMode = model.PaymentMode == null ? "" : model.PaymentMode;
                model.Narration = model.Narration == null ? "" : model.Narration;
                model.AmountOutstanding = model.AmountOutstanding == null ? 0.00m : model.AmountOutstanding.Value;
                model.Comments = "Record Captured by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";
                var record = new TransactionTable()
                {
                    TID = model.TID,
                    FID = model.FID,
                    AmountPaid = model.AmountPaid,
                    AmountOutstanding = model.AmountOutstanding.Value,
                    AccountID = model.AccountID,
                    Bank = bankName,
                    EffectiveDate = model.EffectiveDate,
                    DealerID = model.DealerID,
                    CapturedDate = model.CapturedDate,
                    CapturedBy = model.CapturedBy,
                    PaymentMode = model.PaymentMode,
                    Narration = model.Narration,
                    Comments = model.Comments
                };
                DataContext.TransactionTables.Add(record);

                //Insert Into ProfitAndLoss
                if (model.AmountOutstanding == 0.00m)
                {
                    var thisDate = DateTime.Now.Date;
                    var dateToSplit = DateTime.Now.Date.ToString("dddd/MMMM/yyyy");
                    var thisDay = dateToSplit.Split(new string[] { "/"},StringSplitOptions.RemoveEmptyEntries)[0];
                    var thisMonth = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    var thisYear = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[2];

                    var ProfitAndLossRecordSavedForThisDate = DataContext.ProfitAndLosses.Where(x => x.CapturedDate == thisDate).FirstOrDefault();
                    
                    //If there is no record at all
                    if (ProfitAndLossRecordSavedForThisDate == null)
                    {
                        var newProfitAndLossRecord = new ProfitAndLoss()
                        {
                            TotalTransactionAmount = account.AmountInAccount,
                            TotalExpense = model.AmountPaid,
                            TotalIncome = 0.00m,
                            CapturedDay = thisDay,
                            CapturedMonth = thisMonth,
                            CapturedYear = thisYear,
                            CapturedDate = thisDate,
                            Profit = 0.00m,
                            Loss = 0.00m
                        };
                        DataContext.ProfitAndLosses.Add(newProfitAndLossRecord);
                    }
                    else
                    {
                        //Update's record's expense field
                        ProfitAndLossRecordSavedForThisDate.TotalExpense += model.AmountPaid;
                    }


                    //For Annual
                    var monthOfOperation = DataContext.Setups.Where(x => x.SetUpID == 1).FirstOrDefault().MonthOfOperation;
                    var annual = DataContext.Annuals.Where(x => x.Month == monthOfOperation && x.Year == thisYear).FirstOrDefault();
                    if (annual != null)
                    {
                        //Update record in Annual
                        annual.TotalExpense += model.AmountPaid;
                    }
                    else
                    {
                        var item = new Annual()
                        {
                            Month = monthOfOperation,
                            Year = thisYear,
                            TotalTransactionAmount = account.AmountInAccount,
                            TotalExpense = model.AmountPaid,
                            TotalIncome = 0.00m,
                            Profit = 0.00m,
                            Loss = 0.00m
                        };
                        DataContext.Annuals.Add(item);
                    }
                }


                //Update Account
                totalAmount = record.AmountPaid;
                account.AmountInAccount -= totalAmount;


                SaveAll();

                //Add Debt if Amount Outstanding is present
                var dealerName = DataContext.DealerTables.Where(x => x.DealerID == model.DealerID).FirstOrDefault().DealerName;
                if (model.AmountOutstanding > 0.00m)
                {
                    var debt = new Debt()
                    {
                        TransactionID = record.TransactionID,
                        DeptName = dealerName,
                        Amount = model.AmountOutstanding.Value,
                        EffectiveDate = DateTime.Now,
                        Status = "P"
                    };
                    AddDebt(debt);
                }
                return record.TransactionID;
            }
            #endregion

            #region For Income Transactions
            if (model.TID == "IN")
            {
                model.CapturedBy = model.CapturedBy == null ? "" : model.CapturedBy;
                model.PaymentMode = model.PaymentMode == null ? "" : model.PaymentMode;
                model.Narration = model.Narration == null ? "" : model.Narration;
                model.AmountOutstanding = model.AmountOutstanding == null ? 0.00m : model.AmountOutstanding.Value;
                model.Comments = "Record Captured by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";
                var record = new TransactionTable()
                {
                    TID = model.TID,
                    FID = model.FID,
                    AmountPaid = model.AmountPaid,
                    AmountOutstanding = model.AmountOutstanding.Value,
                    AccountID = model.AccountID,
                    Bank = bankName,
                    EffectiveDate = model.EffectiveDate,
                    DealerID = model.DealerID,
                    CapturedDate = model.CapturedDate,
                    CapturedBy = model.CapturedBy,
                    PaymentMode = model.PaymentMode,
                    Narration = model.Narration,
                    Comments = model.Comments
                };
                DataContext.TransactionTables.Add(record);

                //Insert Into ProfitAndLoss
                if (model.AmountOutstanding == 0.00m)
                {

                    var thisDate = DateTime.Now.Date;
                    var dateToSplit = DateTime.Now.Date.ToString("dddd/MMMM/yyyy");
                    var thisDay = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var thisMonth = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    var thisYear = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[2];

                    var ProfitAndLossRecordSavedForThisDate = DataContext.ProfitAndLosses.Where(x => x.CapturedDate == thisDate).FirstOrDefault();

                    //If there is no record at all
                    if (ProfitAndLossRecordSavedForThisDate == null)
                    {
                        var newProfitAndLossRecord = new ProfitAndLoss()
                        {
                            TotalTransactionAmount = account.AmountInAccount,
                            TotalExpense = 0.00m,
                            TotalIncome = model.AmountPaid,
                            CapturedDay = thisDay,
                            CapturedMonth = thisMonth,
                            CapturedYear = thisYear,
                            CapturedDate = thisDate,
                            Profit = 0.00m,
                            Loss = 0.00m
                        };
                        DataContext.ProfitAndLosses.Add(newProfitAndLossRecord);
                    }
                    else
                    {
                        //Update's record's income field
                        ProfitAndLossRecordSavedForThisDate.TotalIncome += model.AmountPaid;
                    }

                    //For Annual
                    var monthOfOperation = DataContext.Setups.Where(x => x.SetUpID == 1).FirstOrDefault().MonthOfOperation;
                    var annual = DataContext.Annuals.Where(x => x.Month == monthOfOperation && x.Year == thisYear).FirstOrDefault();
                    if (annual != null)
                    {
                        //Update record in Annual
                        annual.TotalIncome += model.AmountPaid;
                    }
                    else
                    {
                        var item = new Annual()
                        {
                            Month = monthOfOperation,
                            Year = thisYear,
                            TotalTransactionAmount = account.AmountInAccount,
                            TotalExpense = 0.00m,
                            TotalIncome = model.AmountPaid,
                            Profit = 0.00m,
                            Loss = 0.00m
                        };
                        DataContext.Annuals.Add(item);
                    }
                }

                //Update Account
                totalAmount = record.AmountPaid;
                account.AmountInAccount += totalAmount;

                SaveAll();

                //Add Debtor if Amount Outstanding is present
                var dealerName = DataContext.DealerTables.Where(x => x.DealerID == model.DealerID).FirstOrDefault().DealerName;
                if (model.AmountOutstanding > 0.00m)
                {
                    var debtor = new Debtor()
                    {
                        TransactionID = record.TransactionID,
                        DeptorName = dealerName,
                        Amount = model.AmountOutstanding.Value,
                        EffectiveDate = DateTime.Now,
                        Status = "P"
                    };
                    AddDebtor(debtor);
                }
                return record.TransactionID;
            }
            #endregion

            return -1;
        }

        public void DeleteASingleTransaction(int transactionID, string user)
        {
            var transaction = DataContext.TransactionTables.Find(transactionID);
            DataContext.TransactionTables.Remove(transaction);

            //Log it
            var date = DateTime.Now.Date;
            var log = new EventLog()
            {
                EventType = "Delete",
                Event = $"The transaction with transaction id: {transactionID} was deleted by {user} on {DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt")}.",
                Executioner = user,
                ExecutionDate = date
            };
            DataContext.EventLogs.Add(log);

            //Remove Debt or Debtor associacted with transaction
            var debt = DataContext.Debts.Where(x => x.TransactionID == transactionID).FirstOrDefault();
            if (debt != null)
            {
                DataContext.Debts.Remove(debt);
            }
            var debtor = DataContext.Debtors.Where(x => x.TransactionID == transactionID).FirstOrDefault();
            if (debtor != null)
            {
                DataContext.Debtors.Remove(debtor);
            }
            SaveAll();
        }

        public void DeleteGroupOfTransactions(string[] transactionIDs, string user)
        {
            foreach (var ID in transactionIDs)
            {
                int transactionID = int.Parse(ID);
                var transaction = DataContext.TransactionTables.Find(transactionID);
                DataContext.TransactionTables.Remove(transaction);

                //Log it
                var date = DateTime.Now.Date;
                var log = new EventLog()
                {
                    EventType = "Delete",
                    Event = $"The transaction with transaction id: {transactionID} was deleted by {user} on {DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt")}.",
                    Executioner = user,
                    ExecutionDate = date
                };
                DataContext.EventLogs.Add(log);

                //Remove Debt or Debtor associacted with transaction
                var debt = DataContext.Debts.Where(x => x.TransactionID == transactionID).FirstOrDefault();
                if (debt != null)
                {
                    DataContext.Debts.Remove(debt);
                }
                var debtor = DataContext.Debtors.Where(x => x.TransactionID == transactionID).FirstOrDefault();
                if (debtor != null)
                {
                    DataContext.Debtors.Remove(debtor);
                }
            }
            SaveAll();
        }

        public TransactionTable FindTransaction(int id)
        {
            return DataContext.TransactionTables.Find(id);
        }

        public IQueryable<TransactionTable> GetAllTransactions()
        {
            return DataContext.TransactionTables;
        }

        public IQueryable<TransactionTypeTable> GetAllTransactionTypes()
        {
            return DataContext.TransactionTypeTables;
        }

        public decimal GetAmountInBank(int accountID)
        {
            var account = DataContext.AccountTables.Find(accountID);
            return account.AmountInAccount;
        }

        public void ReverseAction(int id)
        {
            var transaction = DataContext.TransactionTables.Find(id);
            DataContext.TransactionTables.Remove(transaction);
            SaveAll();
        }

        public int UpdateTransaction(TransactionModel model)
        {
            decimal totalAmount = 0.00m;
            model.AmountOutstanding = model.AmountOutstanding == null ? 0.00m : model.AmountOutstanding;

            var recordToUpdate = DataContext.TransactionTables.Find(model.TransactionID);
            string bankName = null;
            var account = DataContext.AccountTables.Where(x => x.AccountID == model.AccountID).FirstOrDefault();
            bankName = DataContext.Banks.Where(x => x.ID_Bank == account.BankName).FirstOrDefault().BankName;
            decimal previousAmountInBank_EX = account.AmountInAccount + recordToUpdate.AmountPaid;
            decimal previousAmountInBank_IN = account.AmountInAccount - recordToUpdate.AmountPaid;

            decimal previousAmountOutstanding = recordToUpdate.AmountOutstanding.Value;

            model.Comments = model.Comments = "Record Modified by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";

            #region For Expense Transactions
            if (model.TID == "EX")
            {
                recordToUpdate.AccountID = model.AccountID == 0 ? recordToUpdate.AccountID : model.AccountID;
                recordToUpdate.Bank = bankName == null ? recordToUpdate.Bank : bankName;
                recordToUpdate.AmountOutstanding = model.AmountOutstanding.Value;
                recordToUpdate.AmountPaid = model.AmountPaid;
                recordToUpdate.DealerID = model.DealerID == 0 ? recordToUpdate.DealerID : model.DealerID;
                recordToUpdate.EffectiveDate = model.EffectiveDate == null ? recordToUpdate.EffectiveDate : model.EffectiveDate;
                recordToUpdate.FID = model.FID == 0 ? recordToUpdate.FID : model.FID;
                recordToUpdate.Narration = model.Narration == null || model.Narration == "" ? recordToUpdate.Narration : model.Narration;
                recordToUpdate.PaymentMode = model.PaymentMode == null || model.PaymentMode == "" ? recordToUpdate.PaymentMode : model.PaymentMode;
                recordToUpdate.TID = model.TID == null || model.TID == "" ? recordToUpdate.TID : model.TID;
                recordToUpdate.Comments = $"{model.Comments} {recordToUpdate.Comments}";

                //Log it
                var date = DateTime.Now.Date;
                var log = new EventLog()
                {
                    EventType = "Update",
                    Event = $"The transaction with transaction id: {recordToUpdate.TransactionID} was updated by {model.CapturedBy} on {DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt")}.",
                    Executioner = model.CapturedBy,
                    ExecutionDate = date
                };
                DataContext.EventLogs.Add(log);

                //Compute Profit And Loss
                if(recordToUpdate.AmountOutstanding == 0.00m)
                {
                    var thisDate = DateTime.Now.Date;
                    var dateToSplit = DateTime.Now.Date.ToString("dddd/MMMM/yyyy");
                    var thisDay = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var thisMonth = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    var thisYear = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[2];

                    var profitAndLossRecordForThisDate = DataContext.ProfitAndLosses.Where(x => x.CapturedDate == thisDate).FirstOrDefault();
                    if (profitAndLossRecordForThisDate == null)
                    {
                        var newProfitAndLossRecord = new ProfitAndLoss()
                        {
                            TotalTransactionAmount = account.AmountInAccount,
                            TotalExpense = previousAmountOutstanding,
                            TotalIncome = 0.00m,
                            CapturedDay = thisDay,
                            CapturedMonth = thisMonth,
                            CapturedYear = thisYear,
                            CapturedDate = thisDate,
                            Profit = 0.00m,
                            Loss = 0.00m
                        };
                        DataContext.ProfitAndLosses.Add(newProfitAndLossRecord);
                    }
                    else
                    {
                        profitAndLossRecordForThisDate.TotalExpense += previousAmountOutstanding;
                    }


                    //For Annual
                    var monthOfOperation = DataContext.Setups.Where(x => x.SetUpID == 1).FirstOrDefault().MonthOfOperation;
                    var annual = DataContext.Annuals.Where(x => x.Month == monthOfOperation && x.Year == thisYear).FirstOrDefault();
                    if (annual != null)
                    {
                        //Update record in Annual
                        annual.TotalExpense += model.AmountPaid;
                    }
                    else
                    {
                        var item = new Annual()
                        {
                            Month = monthOfOperation,
                            Year = thisYear,
                            TotalTransactionAmount = account.AmountInAccount,
                            TotalExpense = model.AmountPaid,
                            TotalIncome = 0.00m,
                            Profit = 0.00m,
                            Loss = 0.00m
                        };
                        DataContext.Annuals.Add(item);
                    }
                }

                //Update Account
                totalAmount = model.AmountPaid;
                previousAmountInBank_EX -= totalAmount;
                account.AmountInAccount = previousAmountInBank_EX;

                //Update Debt Record
                var debt = DataContext.Debts.Where(x => x.TransactionID == recordToUpdate.TransactionID).FirstOrDefault();
                if (debt != null)
                {
                    if (recordToUpdate.AmountOutstanding.Value == 0.00m)
                    {
                        debt.Status = debt.Status == "P" ? "C" : debt.Status;
                    }
                    if (recordToUpdate.AmountOutstanding.Value > 0.00m && recordToUpdate.AmountOutstanding.Value != previousAmountOutstanding)
                    {
                        debt.Amount = recordToUpdate.AmountOutstanding.Value;
                    }
                }

                SaveAll();
                return recordToUpdate.TransactionID;
            }
            #endregion

            #region For Income Transactions
            if (model.TID == "IN")
            {
                recordToUpdate.AccountID = model.AccountID == 0 ? recordToUpdate.AccountID : model.AccountID;
                recordToUpdate.Bank = bankName == null ? recordToUpdate.Bank : bankName;
                recordToUpdate.AmountOutstanding = model.AmountOutstanding;
                recordToUpdate.AmountPaid = model.AmountPaid;
                recordToUpdate.DealerID = model.DealerID == 0 ? recordToUpdate.DealerID : model.DealerID;
                recordToUpdate.EffectiveDate = model.EffectiveDate == null ? recordToUpdate.EffectiveDate : model.EffectiveDate;
                recordToUpdate.FID = model.FID == 0 ? recordToUpdate.FID : model.FID;
                recordToUpdate.Narration = model.Narration == null || model.Narration == "" ? recordToUpdate.Narration : model.Narration;
                recordToUpdate.PaymentMode = model.PaymentMode == null || model.PaymentMode == "" ? recordToUpdate.PaymentMode : model.PaymentMode;
                recordToUpdate.TID = model.TID == null || model.TID == "" ? recordToUpdate.TID : model.TID;
                recordToUpdate.Comments = $"{model.Comments} {recordToUpdate.Comments}";

                //Log it
                var date = DateTime.Now.Date;
                var log = new EventLog()
                {
                    EventType = "Update",
                    Event = $"The transaction with transaction id: {recordToUpdate.TransactionID} was updated by {model.CapturedBy} on {DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt")}.",
                    Executioner = model.CapturedBy,
                    ExecutionDate = date
                };
                DataContext.EventLogs.Add(log);

                //Compute Profit And Loss
                if (recordToUpdate.AmountOutstanding == 0.00m)
                {
                    var thisDate = DateTime.Now.Date;
                    var dateToSplit = DateTime.Now.Date.ToString("dddd/MMMM/yyyy");
                    var thisDay = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var thisMonth = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    var thisYear = dateToSplit.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[2];

                    var profitAndLossRecordForThisDate = DataContext.ProfitAndLosses.Where(x => x.CapturedDate == thisDate).FirstOrDefault();
                    if (profitAndLossRecordForThisDate == null)
                    {
                        var newProfitAndLossRecord = new ProfitAndLoss()
                        {
                            TotalTransactionAmount = account.AmountInAccount,
                            TotalExpense = 0.00m,
                            TotalIncome = previousAmountOutstanding,
                            CapturedDay = thisDay,
                            CapturedMonth = thisMonth,
                            CapturedYear = thisYear,
                            CapturedDate = thisDate,
                            Profit = 0.00m,
                            Loss = 0.00m
                        };
                        DataContext.ProfitAndLosses.Add(newProfitAndLossRecord);
                    }
                    else
                    {
                        profitAndLossRecordForThisDate.TotalIncome += previousAmountOutstanding;
                    }


                    //For Annual
                    var monthOfOperation = DataContext.Setups.Where(x => x.SetUpID == 1).FirstOrDefault().MonthOfOperation;
                    var annual = DataContext.Annuals.Where(x => x.Month == monthOfOperation && x.Year == thisYear).FirstOrDefault();
                    if (annual != null)
                    {
                        //Update record in Annual
                        annual.TotalIncome += model.AmountPaid;
                    }
                    else
                    {
                        var item = new Annual()
                        {
                            Month = monthOfOperation,
                            Year = thisYear,
                            TotalTransactionAmount = account.AmountInAccount,
                            TotalExpense = 0.00m,
                            TotalIncome = model.AmountPaid,
                            Profit = 0.00m,
                            Loss = 0.00m
                        };
                        DataContext.Annuals.Add(item);
                    }
                }

                //Update Account
                totalAmount = model.AmountPaid;
                previousAmountInBank_IN += totalAmount;
                account.AmountInAccount = previousAmountInBank_IN;

                //Update Debtor Record
                var debtor = DataContext.Debtors.Where(x => x.TransactionID == recordToUpdate.TransactionID).FirstOrDefault();
                if (debtor != null)
                {
                    if (recordToUpdate.AmountOutstanding.Value == 0.00m)
                    {
                        debtor.Status = debtor.Status == "P" ? "C" : debtor.Status;
                    }
                    if (recordToUpdate.AmountOutstanding.Value > 0.00m && recordToUpdate.AmountOutstanding.Value != previousAmountOutstanding)
                    {
                        debtor.Amount = recordToUpdate.AmountOutstanding.Value;
                    }
                }

                SaveAll();
                return recordToUpdate.TransactionID;
            }
            #endregion

            return -1;
        }

        private void SaveAll()
        {
            DataContext.SaveChanges();
        }
    }
}

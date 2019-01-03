using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer;
using eTMS.Repositories;
using eTMS.Repositories.Interfaces;

namespace eTMS.Repositories.Repositories
{
    public class LookUpRepo : BaseRepository<eTMSEntities>,ILookUpRepo
    {
        public IQueryable<AccountTable> GetAllAccounts()
        {
            return DataContext.AccountTables;
        }

        public IQueryable<DealerTable> GetAllDealers()
        {
            return DataContext.DealerTables;
        }

        public IQueryable<Debtor> GetAllDebtors()
        {
            return DataContext.Debtors;
        }

        public IQueryable<Debt> GetAllDebts()
        {
            return DataContext.Debts;
        }

        public IQueryable<TransactionFactorTable> GetAllFactors()
        {
            return DataContext.TransactionFactorTables;
        }

        public IQueryable<TransactionTable> GetAllTransactions()
        {
            return DataContext.TransactionTables;
        }

        public IQueryable<TransactionTypeTable> GetAllTransactionTypes()
        {
            return DataContext.TransactionTypeTables;
        }

        public IQueryable<ProfitAndLoss> GetAllProfitAndLossRecords()
        {
            return DataContext.ProfitAndLosses;
        }

        public decimal GetTotalAmountForResolvedDebts()
        {
            var count = DataContext.Debts.Count(x => x.Status == "C");
            if(count > 0)
            {
                return DataContext.Debts.Where(x => x.Status == "C").Sum(x => x.Amount);
            }
            return 0.00m;
        }

        public decimal GetTotalAmountForAccounts()
        {
            var count = DataContext.AccountTables.Count();
            if(count > 0)
            {
                return DataContext.AccountTables.Sum(x => x.AmountInAccount);
            }
            return 0.00m;
        }

        public decimal GetTotalAmountForDebtors()
        {
            var count =  DataContext.Debtors.Count(x => x.Status == "P");
            if(count > 0)
            {
                return DataContext.Debtors.Where(x => x.Status == "P").Sum(x => x.Amount);
            }
            return 0.00m;
        }

        public decimal GetTotalAmountForDebts()
        {
            var count = DataContext.Debts.Count(x => x.Status == "P");
            if(count > 0)
            {
                return DataContext.Debts.Where(x => x.Status == "P").Sum(x => x.Amount);
            }
            return 0.00m;
        }

        public decimal GetTotalAmountForReceipts()
        {
            var count = DataContext.TransactionTables.Count(x => x.AmountOutstanding == 0.00m);
            if(count > 0)
            {
                return DataContext.TransactionTables.Where(x => x.AmountOutstanding == 0.00m).Sum(x => x.AmountPaid);
            }
            return 0.00m;
        }

        public decimal GetTotalAmountForResolvedDebtors()
        {
            var count = DataContext.Debtors.Count(x => x.Status == "C");
            if(count > 0)
            {
                return DataContext.Debtors.Where(x => x.Status == "C").Sum(x => x.Amount);
            }
            return 0.00m;
        }

        public decimal GetTotalAmountForTransactions()
        {
            var count = DataContext.TransactionTables.Count();
            if(count > 0)
            {
                return DataContext.TransactionTables.Sum(x => x.AmountPaid);
            }
            return 0.00m;
        }

        public decimal GetTotalProfit()
        {
            var count = DataContext.ProfitAndLosses.Count();
            if (count > 0)
            {
                return DataContext.ProfitAndLosses.Sum(x => x.Profit);
            }
            return 0.00m;
        }

        public decimal GetTotalLoss()
        {
            var count = DataContext.ProfitAndLosses.Count();
            if (count > 0)
            {
                return DataContext.ProfitAndLosses.Sum(x => x.Loss);
            }
            return 0.00m;
        }

        public IQueryable<TransactionTable> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate)
        {
            return DataContext.TransactionTables.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
        }

        public IQueryable<AccountTable> GetAccountsBetweenDates(DateTime startDate, DateTime endDate)
        {
            return DataContext.AccountTables.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
        }

        public IQueryable<Debtor> GetDebtorsBetweenDates(DateTime startDate, DateTime endDate)
        {
            return DataContext.Debtors.Where(x=>x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
        }

        public IQueryable<DealerTable> GetDealersBetweenDates(DateTime startDate, DateTime endDate)
        {
            return DataContext.DealerTables.Where(x=>x.CapturedDate >= startDate && x.CapturedDate <= endDate);
        }

        public IQueryable<Debt> GetDebtsBetweenDates(DateTime startDate, DateTime endDate)
        {
            return DataContext.Debts.Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate);
        }

        public IQueryable<ProfitAndLoss> GetProfitAndLossRecordsBetweenDates(DateTime startDate, DateTime endDate)
        {
            return DataContext.ProfitAndLosses.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
        }

        public IQueryable<EventLog> GetAllEvents()
        {
            return DataContext.EventLogs;
        }

        public IQueryable<EventLog> GetEventsByDate(DateTime date)
        {
            return DataContext.EventLogs.Where(x => x.ExecutionDate == date);
        }
    }
}

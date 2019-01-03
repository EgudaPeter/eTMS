using eTMS.BusinessObjectLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTMS.Repositories.Interfaces
{
    public interface ILookUpRepo
    {
        IQueryable<AccountTable> GetAllAccounts();

        IQueryable<DealerTable> GetAllDealers();

        IQueryable<TransactionFactorTable> GetAllFactors();

        IQueryable<TransactionTable> GetAllTransactions();

        IQueryable<EventLog> GetAllEvents();

        IQueryable<Debt> GetAllDebts();

        IQueryable<Debtor> GetAllDebtors();

        IQueryable<TransactionTypeTable> GetAllTransactionTypes();

        IQueryable<ProfitAndLoss> GetAllProfitAndLossRecords();

        IQueryable<EventLog> GetEventsByDate(DateTime date);

        IQueryable<TransactionTable> GetTransactionsBetweenDates(DateTime startDate, DateTime endDate);

        IQueryable<AccountTable> GetAccountsBetweenDates(DateTime startDate, DateTime endDate);

        IQueryable<Debtor> GetDebtorsBetweenDates(DateTime startDate, DateTime endDate);

        IQueryable<DealerTable> GetDealersBetweenDates(DateTime startDate, DateTime endDate);

        IQueryable<Debt> GetDebtsBetweenDates(DateTime startDate, DateTime endDate);

        IQueryable<ProfitAndLoss> GetProfitAndLossRecordsBetweenDates(DateTime startDate, DateTime endDate);

        decimal GetTotalAmountForTransactions();

        decimal GetTotalAmountForReceipts();

        decimal GetTotalAmountForAccounts();

        decimal GetTotalAmountForDebtors();

        decimal GetTotalAmountForResolvedDebtors();

        decimal GetTotalAmountForDebts();

        decimal GetTotalAmountForResolvedDebts();

        decimal GetTotalProfit();

        decimal GetTotalLoss();

    }
}

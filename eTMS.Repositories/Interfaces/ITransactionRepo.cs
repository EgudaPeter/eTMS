using eTMS.BusinessObjectLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer.Models;

namespace eTMS.Repositories.Interfaces
{
    public interface ITransactionRepo
    {
        IQueryable<TransactionTable> GetAllTransactions();

        IQueryable<TransactionTypeTable> GetAllTransactionTypes();

        int AddTransaction(TransactionModel model);

        int UpdateTransaction(TransactionModel model);

        TransactionTable FindTransaction(int id);

        void DeleteASingleTransaction(int transactionID, string user);

        void DeleteGroupOfTransactions(string[] transactionIDs, string user);

        decimal GetAmountInBank(int accountID);

        void AddDebt(Debt debt);

        void AddDebtor(Debtor debtor);

        void ReverseAction(int id);
    }
}

using eTMS.BusinessObjectLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTMS.Repositories.Interfaces
{
    public interface IAccountRepo
    {
        IQueryable<AccountTable> GetAllAccounts();

        IQueryable<Bank> GetAllBanks();

        int AddAccount(AccountTable model);

        int UpdateAccount(AccountTable model);

        AccountTable FindAccount(int id);

        bool CheckIfAccountNumberIsUnique(string accountNumber);

        void DeleteASingleAccount(int accountID);

        void DeleteGroupOfAccounts(string[] accountIDs);
    }
}

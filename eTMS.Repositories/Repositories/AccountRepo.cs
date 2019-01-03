using eTMS.BusinessObjectLayer;
using eTMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTMS.Repositories.Repositories
{
    public class AccountRepo : BaseRepository<eTMSEntities>, IAccountRepo
    {
        public int AddAccount(AccountTable model)
        {
            model.Comments = "Record Captured by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";
            DataContext.AccountTables.Add(model);
            SaveAll();
            return model.AccountID;
        }

        public bool CheckIfAccountNumberIsUnique(string accountNumber)
        {
            if(DataContext.AccountTables.Any(x=>x.AccountNumber == accountNumber))
            {
                return false;
            }
            return true;
        }

        public void DeleteASingleAccount(int accountID)
        {
            var account = DataContext.AccountTables.Find(accountID);
            DataContext.AccountTables.Remove(account);
            SaveAll();
        }

        public void DeleteGroupOfAccounts(string[] accountIDs)
        {
            foreach (var ID in accountIDs)
            {
                int accountID = int.Parse(ID);
                var account = DataContext.AccountTables.Find(accountID);
                DataContext.AccountTables.Remove(account);
            }
            SaveAll();
        }

        public AccountTable FindAccount(int id)
        {
            return DataContext.AccountTables.Find(id);
        }

        public IQueryable<AccountTable> GetAllAccounts()
        {
            return DataContext.AccountTables;
        }

        public IQueryable<Bank> GetAllBanks()
        {
            return DataContext.Banks;
        }

        public int UpdateAccount(AccountTable model)
        {
            model.Comments = "Record Modified by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";
            var record = DataContext.AccountTables.Where(x => x.AccountID == model.AccountID).FirstOrDefault();
            record.AccountName = model.AccountName;
            record.AccountNumber = model.AccountNumber;
            record.BankName = model.BankName;
            record.AmountInAccount = model.AmountInAccount;
            record.Comments = $"{model.Comments} {record.Comments}";
            SaveAll();
            return record.AccountID;
        }

        private void SaveAll()
        {
            DataContext.SaveChanges();
        }
    }
}

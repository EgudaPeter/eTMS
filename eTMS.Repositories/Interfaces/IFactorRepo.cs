using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer;

namespace eTMS.Repositories.Interfaces
{
    public interface IFactorRepo
    {
        IQueryable<TransactionFactorTable> GetAllFactors();

        int AddFactor(TransactionFactorTable model);

        int UpdateFactor(TransactionFactorTable model);

        TransactionFactorTable FindFactor(int id);

        bool CheckIfFactorNameIsUnique(string factorName);

        void DeleteASingleFactor(int factorID);

        void DeleteGroupOfFactors(string[] factorIDs);
    }
}

using eTMS.BusinessObjectLayer;
using System.Linq;

namespace eTMS.Repositories.Interfaces
{
    public interface IDealersRepo
    {
        IQueryable<DealerTable> GetAllDealers();

        int AddDealer(DealerTable model);

        int UpdateDealer(DealerTable model);

        DealerTable FindDealer(int id);

        bool CheckIfDealerNameIsUnique(string dealerName);

        void DeleteASingleDealer(int dealerID);

        void DeleteGroupOfDealers(string[] dealerIDs);
    }
}

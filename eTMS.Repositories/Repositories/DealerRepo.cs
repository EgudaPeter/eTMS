using eTMS.BusinessObjectLayer;
using eTMS.Repositories.Interfaces;
using System;
using System.Linq;

namespace eTMS.Repositories.Repositories
{
    public class DealerRepo : BaseRepository<eTMSEntities>, IDealersRepo
    {
        public int AddDealer(DealerTable model)
        {
            model.Comments = "Record Captured by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";
            DataContext.DealerTables.Add(model);
            SaveAll();
            return model.DealerID;
        }

        public bool CheckIfDealerNameIsUnique(string dealerName)
        {
            if (DataContext.DealerTables.Any(x => x.DealerName == dealerName))
            {
                return false;
            }
            return true;
        }

        public void DeleteASingleDealer(int dealerID)
        {
            var dealer = DataContext.DealerTables.Find(dealerID);
            DataContext.DealerTables.Remove(dealer);
            SaveAll();
        }

        public void DeleteGroupOfDealers(string[] dealerIDs)
        {
            foreach (var ID in dealerIDs)
            {
                int dealerID = int.Parse(ID);
                var dealer = DataContext.DealerTables.Find(dealerID);
                DataContext.DealerTables.Remove(dealer);
            }
            SaveAll();
        }

        public DealerTable FindDealer(int id)
        {
            return DataContext.DealerTables.Find(id);
        }

        public IQueryable<DealerTable> GetAllDealers()
        {
            return DataContext.DealerTables;
        }

        public int UpdateDealer(DealerTable model)
        {
            model.Comments = "Record Modified by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";
            var record = DataContext.DealerTables.Where(x => x.DealerID == model.DealerID).FirstOrDefault();
            record.DealerName = model.DealerName;
            record.Comments = $"{model.Comments} {record.Comments}";
            SaveAll();
            return record.DealerID;
        }

        private void SaveAll()
        {
            DataContext.SaveChanges();
        }
    }
}

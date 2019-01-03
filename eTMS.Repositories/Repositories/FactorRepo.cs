using eTMS.Repositories.Interfaces;
using System.Linq;
using eTMS.BusinessObjectLayer;
using System;

namespace eTMS.Repositories.Repositories
{
    public class FactorRepo : BaseRepository<eTMSEntities>, IFactorRepo
    {
        public int AddFactor(TransactionFactorTable model)
        {
            model.Comments = model.Comments = "Record Captured by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";
            DataContext.TransactionFactorTables.Add(model);
            SaveAll();
            return model.FID;
        }

        public bool CheckIfFactorNameIsUnique(string factorName)
        {
           if(DataContext.TransactionFactorTables.Any(x=>x.Name == factorName))
            {
                return false;
            }
            return true;
        }

        public void DeleteASingleFactor(int factorID)
        {
            var factor = DataContext.TransactionFactorTables.Find(factorID);
            DataContext.TransactionFactorTables.Remove(factor);
            SaveAll();
        }

        public void DeleteGroupOfFactors(string[] factorIDs)
        {
            foreach(var ID in factorIDs)
            {
                int fID = int.Parse(ID);
                var factor = DataContext.TransactionFactorTables.Find(fID);
                DataContext.TransactionFactorTables.Remove(factor);
            }
            SaveAll();
        }

        public TransactionFactorTable FindFactor(int id)
        {
            return DataContext.TransactionFactorTables.Find(id);
        }

        public IQueryable<TransactionFactorTable> GetAllFactors()
        {
            return DataContext.TransactionFactorTables;
        }

        public int UpdateFactor(TransactionFactorTable model)
        {
            model.Comments = "Record Modified by " + Environment.NewLine + model.CapturedBy + Environment.NewLine + " on " + DateTime.Now.ToString("dd MMMM yyyy h:mm:ss tt") + Environment.NewLine + "==============================";
            var record = DataContext.TransactionFactorTables.Where(x => x.FID == model.FID).FirstOrDefault();
            record.Name = model.Name;
            record.Comments = $"{model.Comments} {record.Comments}";
            SaveAll();
            return record.FID;
        }

        private void SaveAll()
        {
            DataContext.SaveChanges();
        }
    }
}

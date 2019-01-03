using System;
using System.Linq;
using eTMS.BusinessObjectLayer;
using eTMS.Repositories.Interfaces;

namespace eTMS.Repositories.Repositories
{
    public class ProfitAndLossRepo : BaseRepository<eTMSEntities>, IProfitAndLossRepo
    {
        public void ComputeProfitAndLossForPreviousDay(DateTime date)
        {
            var record = DataContext.ProfitAndLosses.Where(x => x.CapturedDate == date).FirstOrDefault();
            if(record != null)
            {
                if (record.Profit == 0.00m && record.Loss == 0.00m)
                {
                    decimal TotalTransactionAmount = record.TotalTransactionAmount;
                    decimal TotalExpense = record.TotalExpense;
                    decimal TotalIncome = record.TotalIncome;
                    decimal profit = 0.00m; decimal loss = 0.00m;

                    decimal result = TotalTransactionAmount + TotalIncome - TotalExpense;

                    if (result > TotalTransactionAmount)
                    {
                        profit = result - TotalTransactionAmount;
                        record.Profit = profit;
                    }
                    if (result < TotalTransactionAmount)
                    {
                        loss = TotalTransactionAmount - result;
                        record.Loss = loss;
                    }

                    var thisDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    var thisYear = thisDate.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[2];
                    var monthOfOperation = DataContext.Setups.Where(x => x.SetUpID == 1).FirstOrDefault().MonthOfOperation;
                    var annual = DataContext.Annuals.Where(x => x.Month == monthOfOperation && x.Year == thisYear).FirstOrDefault();
                    if(annual != null)
                    {
                        annual.Profit += record.Profit;
                        annual.Loss += record.Loss;
                    }


                    SaveAll();
                }
            }
        }

        public bool EnableWeekendProfitAndLossAnalysis()
        {
            var settings = DataContext.Setups.Where(x => x.SetUpID == 1).FirstOrDefault();
            return settings.EnableWeekendProfitAndLossAnalysis.Value;
        }

        public IQueryable<ProfitAndLoss> GetAllProfitAndLossRecords()
        {
            return DataContext.ProfitAndLosses;
        }

        public IQueryable<ProfitAndLoss> GetAllProfitAndLossRecordsBetweenDates(DateTime startDate, DateTime endDate)
        {
            return DataContext.ProfitAndLosses.Where(x => x.CapturedDate >= startDate && x.CapturedDate <= endDate);
        }

        public IQueryable<ProfitAndLoss> GetAllProfitAndLossRecordsForAMonth(string month)
        {
            return DataContext.ProfitAndLosses.Where(x => x.CapturedMonth == month);
        }

        public IQueryable<ProfitAndLoss> GetAllProfitAndLossRecordsForDate(DateTime date)
        {
            DateTime capturedDate = date.Date;
            return DataContext.ProfitAndLosses.Where(x => x.CapturedDate == capturedDate);
        }

        private void SaveAll()
        {
            DataContext.SaveChanges();
        }
    }
}

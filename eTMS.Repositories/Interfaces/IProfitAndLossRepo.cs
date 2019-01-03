using eTMS.BusinessObjectLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTMS.Repositories.Interfaces
{
    public interface IProfitAndLossRepo
    {
        void ComputeProfitAndLossForPreviousDay(DateTime date);

        IQueryable<ProfitAndLoss> GetAllProfitAndLossRecords();

        IQueryable<ProfitAndLoss> GetAllProfitAndLossRecordsForDate(DateTime date);

        IQueryable<ProfitAndLoss> GetAllProfitAndLossRecordsBetweenDates(DateTime startDate, DateTime endDate);

        IQueryable<ProfitAndLoss> GetAllProfitAndLossRecordsForAMonth(string month);

        bool EnableWeekendProfitAndLossAnalysis();
    }
}

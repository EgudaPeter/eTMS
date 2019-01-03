using eTMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer;

namespace eTMS.Repositories.Repositories
{
    public class SetupRepo : BaseRepository<eTMSEntities>, ISetupRepo
    {
        public Setup GetSettings()
        {
            return DataContext.Setups.Where(x=>x.SetUpID == 1).FirstOrDefault();
        }

        public void SaveSettings(string month, bool enableWeekendProfitAndLossAnalysis)
        {
            var settings = DataContext.Setups.Where(x => x.SetUpID == 1).FirstOrDefault();

            settings.MonthOfOperation = month;
            settings.EnableWeekendProfitAndLossAnalysis = enableWeekendProfitAndLossAnalysis;

            DataContext.SaveChanges();
        }
    }
}

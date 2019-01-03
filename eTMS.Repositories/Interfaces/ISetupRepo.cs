using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer;

namespace eTMS.Repositories.Interfaces
{
    public interface ISetupRepo
    {
        void SaveSettings(string month, bool enableWeekendProfitAndLossAnalysis);

        Setup GetSettings();
    }
}

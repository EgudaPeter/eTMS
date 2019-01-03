using eTMS.BusinessObjectLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTMS.Repositories.Interfaces
{
    public interface IAnnualRepo
    {
        IQueryable<Annual> GatSummaryForYear(string year);
    }
}

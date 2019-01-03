using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer;
using eTMS.Repositories.Interfaces;

namespace eTMS.Repositories.Repositories
{
    public class AnnualRepo : BaseRepository<eTMSEntities>, IAnnualRepo
    {
        public IQueryable<Annual> GatSummaryForYear(string year)
        {
            return DataContext.Annuals.Where(x => x.Year == year);
        }
    }
}

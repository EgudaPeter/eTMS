using DevExpress.Data.Linq;
using DevExpress.Web.Mvc;
using eTMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eTMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "SU,AC")]
    [RouteArea("admin")]
    public class AnnualReportController : Controller
    {

        IAnnualRepo annualRepo;
        public AnnualReportController(IAnnualRepo _annualRepo)
        {
            annualRepo = _annualRepo;
        }
        // GET: Admin/AnnualReport
        [Route("profitandloss/annualreport")]
        public ActionResult AnnualReport()
        {
            ViewBag.Years = new SelectList(years().ToList());
            return View();
        }

        private IEnumerable<string> years()
        {
            string[] arry = {
                "1990","1991","1992","1993","1994","1995","1996","1997","1998","1999","2000","2001","2002","2003","2004","2005",
                "2006","2007","2008","2009","2010","2011","2012","2013","2014","2015","2016","2017","2018","2019","2020","2021",
                "2022","2023","2024","2025","2026","2027","2028","2029","2030","2031","2032","2033","2034","2035","2036","2037"
            };
            return arry;
        }

        [ValidateInput(false)]
        public ActionResult AnnualGridViewPartial(string _year)
        {
            return PartialView("_AnnualGridViewPartial", GetEntityDataSource(_year));
        }

        private EntityServerModeSource GetEntityDataSource(string _year)
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "AID";
            if(_year != null)
            {
                esms.QueryableSource = annualRepo.GatSummaryForYear(_year);
                return esms;
            }
            esms.QueryableSource = null;
            return esms;
        }
    }
}
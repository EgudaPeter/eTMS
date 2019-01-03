using eTMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eTMS.Areas.Management.Controllers
{
    [Authorize(Roles ="AC,SA,SU")]
    [RouteArea("management")]
    public class ErrorController : Controller
    {
        // GET: Management/Error
        [Route("applicationerror")]
        public ActionResult ApplicationError()
        {
            if(Settings.error != null)
            {
                ViewBag.ErrorMessage = Settings.error;
            }
            return View();
        }
    }
}
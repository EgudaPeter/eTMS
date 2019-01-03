using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using eTMS.BusinessObjectLayer.Models;
using eTMS.Common;
using System;
using eTMS.Repositories.Interfaces;
using eTMS.Secure.Interfaces;

namespace eTMS.Areas.Secutity.Controllers
{
    [RouteArea("security")]
    [AllowAnonymous]
    public class AuthenticateController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        #endregion

        IUsersRepo userRepo;

        public AuthenticateController(IUsersRepo _userRepo)
        {
            userRepo = _userRepo;
        }

        // GET: Security/Authenticate
        [Route("login")]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Authenticate", new { Area = "Security" });
        }

        [HttpPost]
        public JsonResult doLogin(LoginModel Data)
        {
            try
            {
                var encryptedPassword = Data.Password != null ? Cryptography.EncryptString(Data.Password) : null;
                if(encryptedPassword == null || Data.Username == null)
                {
                    return Json(new
                    {
                        success = false,
                        infoMessage = "Invalid login credentials!"
                    });
                }
                if (Membership.ValidateUser(Data.Username, encryptedPassword))
                {
                    FormsAuthentication.SetAuthCookie(Data.Username, Data.RememeberMe);
                    userRepo.LogUserLogin(Data.Username, encryptedPassword, DateTime.Now);
                    return Json(new { success = true });
                }
                return Json(new
                {
                    success = false,
                    infoMessage = "Invalid login credentials!"
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                var innerEx = ex.InnerException != null ? ex.InnerException.InnerException.Message : "";
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message} {innerEx}"
                });
            }
        }
    }
}
using eTMS.Common;
using eTMS.Secure.Interfaces;
using System;
using System.Web.Mvc;

namespace eTMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "SU,AC,SA")]
    [RouteArea("admin")]
    public class ManagePasswordController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        #endregion

        IUsersRepo userRepo;

        public ManagePasswordController(IUsersRepo _userRepo)
        {
            userRepo = _userRepo;
        }

        // GET: Management/ManagePassword
        [Route("changepassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Modify(string OldPassword, string NewPassword)
        {
            try
            {
                var oldEncryptedPassword = Cryptography.EncryptString(OldPassword);
                var newEncryptedPassword = Cryptography.EncryptString(NewPassword);
                var result = userRepo.ValidateOldPassword(oldEncryptedPassword);
                if (result == true)
                {
                    userRepo.ChangePassword(oldEncryptedPassword, newEncryptedPassword);
                    return Json(new
                    {
                        success = true,
                        infoMessage = $"Password successfully changed!"
                    });
                }
                return Json(new
                {
                    success = false,
                    infoMessage = $"Old password is incorrect!"
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }
    }
}
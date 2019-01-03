using DevExpress.Data.Linq;
using eTMS.BusinessObjectLayer;
using eTMS.Common;
using eTMS.Repositories.Interfaces;
using eTMS.Secure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace eTMS.Areas.Management.Controllers
{
    [Authorize(Roles = "SU")]
    [RouteArea("management")]
    public class UsersController : Controller
    {
        #region Global Declarations
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string errorMessage = "Operation failed due to an internal system error. Contact system administrator.";
        static int tempCreatedID = -1;
        static int tempUpdatedID = -1;
        #endregion

        IUsersRepo userRepo;

        public UsersController( IUsersRepo _userRepo)
        {
             userRepo = _userRepo;
        }

        // GET: UserManagement/Users
        [Route("users")]
        public ActionResult Users()
        {
            ViewBag.Actions = new SelectList(actions().ToList());
            ViewBag.Groups = userRepo.GetAllGroups().ToList();
            return View();
        }

        private IEnumerable<string> actions()
        {
            string[] arry = new string[] { "Select Action", "Delete" };
            return arry.ToList();
        }

        [ValidateInput(false)]
        public ActionResult UsersGridViewPartial()
        {
            if (tempUpdatedID > 0)
            {
                ViewBag.Key = tempUpdatedID;
                tempUpdatedID = -1;
            }
            if (tempCreatedID > 0)
            {
                ViewBag.Key = tempCreatedID;
                tempCreatedID = -1;
            }
            return PartialView("_UsersGridViewPartial", GetEntityDataSource());
        }

        private EntityServerModeSource GetEntityDataSource()
        {
            EntityServerModeSource esms = new EntityServerModeSource();
            esms.KeyExpression = "AdminID";
            var model = from ad in userRepo.GetAllUsers()
                        join u in userRepo.GetAllGroups()
                        on ad.AdminUID equals u.Code
                        select new
                        {
                            AdminID = ad.AdminID,
                            AdminSurname = ad.AdminSurname,
                            AdminFirstname = ad.AdminFirstname,
                            AdminMiddlename = ad.AdminMiddlename,
                            AdminUsername = ad.AdminUsername,
                            AdminPassword = ad.AdminPassword,
                            AdminUID = u.Description,
                            AdminPhone = ad.AdminPhone,
                            CreatedDate = ad.CreatedDate
                        };
            esms.QueryableSource = model;
            return esms;
        }

        #region Functions
        [HttpPost]
        public JsonResult AddUser(AdminTable Data)
        {
            try
            {
                Data.CreatedDate = DateTime.Now;
                Data.AdminPassword = Cryptography.EncryptString(Data.AdminPassword);
                tempCreatedID = userRepo.AddUser(Data);
                return Json(new
                {
                    success = true,
                    infoMessage = $"User added successfully!"
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult UpdateUser(AdminTable Data)
        {
            try
            {
                tempUpdatedID = userRepo.UpdateUser(Data);
                return Json(new
                {
                    success = true,
                    infoMessage = $"User updated successfully!"
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult ValidateGSM(string phone)
        {
            try
            {
                var result = userRepo.CheckIfGSMIsUnique(phone);
                var result2 = userRepo.CheckIfGsmIsInTheRightFormat(phone);
                if (result == true && result2 == true)
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                return Json(new
                {
                    success = false,
                    infoMessage = $"Entered Phone number <b>({phone})</b> exists or is not in the correct format!"
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult ValidateUsername(string _userName)
        {
            try
            {
                var result = userRepo.CheckIfUserNameIsUnique(_userName);
                if (result == true)
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                return Json(new
                {
                    success = false,
                    infoMessage = $"Entered Username <b>({_userName})</b> exists!"
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }
        }

        [HttpPost]
        public JsonResult EditUser(string userID)
        {
            try
            {
                int ID = int.Parse(userID);
                var user = userRepo.FindUser(ID);
                return Json(new
                {
                    success = true,
                    adminID = user.AdminID,
                    adminSurname = user.AdminSurname,
                    adminFirstname = user.AdminFirstname,
                    adminMiddlename = user.AdminMiddlename,
                    adminUsername = user.AdminUsername,
                    adminUID = user.AdminUID,
                    adminPhone = user.AdminPhone
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }           
        }

        [HttpPost]
        public JsonResult DeleteUser(string selectedKey)
        {
            try
            {
                int userId = int.Parse(selectedKey);
                userRepo.DeleteASingleUser(userId);
                return Json(new
                {
                    success = true,
                    infoMessage = $"User successfully deleted!"
                });
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
                return Json(new
                {
                    success = -1,
                    errorMessage = $"{errorMessage} Error Detail:{ex.Message}"
                });
            }  
        }

        [HttpPost]
        public JsonResult DeleteUsers(string selectedKeys)
        {
            try
            {
                string [] userIDS = selectedKeys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                userRepo.DeleteAGroupOfUsers(userIDS);
                return Json(new
                {
                    success = true,
                    infoMessage = $"Users successfully deleted!"
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
        #endregion
    }
}
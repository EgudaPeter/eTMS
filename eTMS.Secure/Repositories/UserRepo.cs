using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer;
using eTMS.Secure.Interfaces;

namespace eTMS.Secure.Repositories
{
    public class UserRepo : BaseSecureRepository<eTMS_SecureEntities>, IUsersRepo
    {
        public int AddUser(AdminTable model)
        {
            model.AdminMiddlename = model.AdminMiddlename == null ? "" : model.AdminMiddlename;
            DataContext.AdminTables.Add(model);
            SaveAll();
            return model.AdminID;
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            var user = DataContext.AdminTables.Where(x => x.AdminPassword == oldPassword).FirstOrDefault();
            user.AdminPassword = newPassword;
            SaveAll();
        }

        public bool CheckIfGsmIsInTheRightFormat(string phone)
        {
            string clientCode = phone.Remove(3);
            var codes = DataContext.GsmValidators;
            foreach (var code in codes)
            {
                if (code.Gsm1st3Digits == clientCode)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfGSMIsUnique(string gsm)
        {
            if (DataContext.AdminTables.Any(x => x.AdminPhone == gsm))
            {
                return false;
            }
            return true;
        }

        public bool CheckIfUserNameIsUnique(string userName)
        {
            if (DataContext.AdminTables.Any(x => x.AdminUsername == userName))
            {
                return false;
            }
            return true;
        }

        public void DeleteAGroupOfUsers(string[] IDs)
        {
            foreach (var id in IDs)
            {
                int userId = int.Parse(id);
                var user = DataContext.AdminTables.Find(userId);
                DataContext.AdminTables.Remove(user);
            }
            SaveAll();
        }

        public void DeleteASingleUser(int ID)
        {
            var user = DataContext.AdminTables.Find(ID);
            DataContext.AdminTables.Remove(user);
            SaveAll();
        }

        public AdminTable FindUser(int ID)
        {
            return DataContext.AdminTables.Find(ID);
        }

        public IQueryable<AdminTable> GetAllUsers()
        {
            return DataContext.AdminTables;
        }

        public IQueryable<UIDTable> GetAllGroups()
        {
            return DataContext.UIDTables;
        }

        public void LogUserLogin(string username, string password, DateTime loginDate)
        {
            var record = new AdminLogin()
            {
                adminUsername = username,
                adminPassword = password,
                adminLoginDate = loginDate
            };
            DataContext.AdminLogins.Add(record);
            SaveAll();
        }

        public int UpdateUser(AdminTable model)
        {
            model.AdminMiddlename = model.AdminMiddlename == null ? "" : model.AdminMiddlename;
            var userToUpdate = DataContext.AdminTables.Find(model.AdminID);

            userToUpdate.AdminSurname = model.AdminSurname;
            userToUpdate.AdminFirstname = model.AdminFirstname;
            userToUpdate.AdminMiddlename = model.AdminMiddlename == "" ? userToUpdate.AdminMiddlename : model.AdminMiddlename;
            userToUpdate.AdminUsername = model.AdminUsername;
            userToUpdate.AdminUID = model.AdminUID;
            userToUpdate.AdminPhone = model.AdminPhone;

            SaveAll();
            return userToUpdate.AdminID;
        }

        public bool ValidateOldPassword(string oldPassword)
        {
            if (DataContext.AdminTables.Any(x => x.AdminPassword == oldPassword))
            {
                return true;
            }
            return false;
        }

        private void SaveAll()
        {
            DataContext.SaveChanges();
        }
    }
}

using eTMS.BusinessObjectLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTMS.Secure.Interfaces
{
    public interface IUsersRepo
    {
        IQueryable<UIDTable> GetAllGroups();

        IQueryable<AdminTable> GetAllUsers();

        int AddUser(AdminTable model);

        int UpdateUser(AdminTable model);

        AdminTable FindUser(int ID);

        bool CheckIfGSMIsUnique(string gsm);

        bool CheckIfUserNameIsUnique(string userName);

        void DeleteASingleUser(int ID);

        void DeleteAGroupOfUsers(string[] IDs);

        void LogUserLogin(string username, string password, DateTime loginDate);

        bool CheckIfGsmIsInTheRightFormat(string phone);

        bool ValidateOldPassword(string oldPassword);

        void ChangePassword(string oldPassword, string newPassword);
    }
}

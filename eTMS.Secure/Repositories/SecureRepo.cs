using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer;
using eTMS.Secure.Interfaces;

namespace eTMS.Secure.Repositories
{
    public class SecureRepo : ISecureRepo
    {
        public string [] GetGroupsUserIsIn(AdminTable user)
        {
            using (var DataContext = new eTMS_SecureEntities())
            {
                string[] groups = { DataContext.UIDTables.Where(x => x.Code == user.AdminUID).FirstOrDefault().Code };
                return groups;
            }  
        }

        public bool FindUser(string username, string password)
        {
            using (var DataContext = new eTMS_SecureEntities())
            {
                if (DataContext.AdminTables.Any(x => x.AdminUsername == username && x.AdminPassword == password))
                {
                    return true;
                }
                return false;
            }
        }

        public IQueryable<UIDTable> GetAllGroups()
        {
            using (var DataContext = new eTMS_SecureEntities())
            {
                return DataContext.UIDTables;
            }            
        }

        public AdminTable GetUser(string username)
        {
            using (var DataContext = new eTMS_SecureEntities())
            {
                var user = DataContext.AdminTables.Where(x => x.AdminUsername == username).FirstOrDefault();
                return user;
            }
        }
    }
}

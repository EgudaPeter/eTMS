using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTMS.BusinessObjectLayer;

namespace eTMS.Secure.Interfaces
{
    public interface ISecureRepo
    {
        bool FindUser(string username, string password);

        AdminTable GetUser(string username);

        IQueryable<UIDTable> GetAllGroups();

        string[] GetGroupsUserIsIn (AdminTable user);
    }
}

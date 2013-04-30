using StatCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace StatCoach.Data
{
    public class UserRepository : IDisposable
    {
        private dbStatCoachEntities db;

        public UserRepository()
        {
            this.db = new dbStatCoachEntities();
        }

        public UserModel GetCurrentUser()
        {
            int userId = WebSecurity.CurrentUserId;
            return this.db.Users.Where(u => u.Id == userId).Select(u => new UserModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).FirstOrDefault();
        }

        public UserModel GetUserById(int userId)
        {
            return this.db.Users.Where(u => u.Id == userId).Select(u => new UserModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).FirstOrDefault();
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            return this.db.Users.Select(u => new UserModel
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            });
        }

        public bool DeleteUser(int userId)
        {
            SimpleMembershipProvider provider = Membership.Provider as SimpleMembershipProvider;

            try
            {
                string userName = this.GetUserById(userId).Email;
                return provider.DeleteUser(userName, true);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
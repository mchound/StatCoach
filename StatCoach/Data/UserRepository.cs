using StatCoach.Business.Interfaces;
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
        private dbStatCoachEntitiesAzure db;

        public UserRepository()
        {
            this.db = new dbStatCoachEntitiesAzure();
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

        public int AddRole(string roleName)
        {
            try
            {
                Roles.CreateRole(roleName);
                webpages_Roles role = db.webpages_Roles.FirstOrDefault(r => r.RoleName == roleName);
                int roleId = role != null ? role.RoleId : -1;
                return roleId;
            }
            catch (Exception ex)
            {
                // TODO: Logging
                return -1;
            }
        }

        public bool AddUserToRole(string userName, string roleName)
        {
            try
            {
                Roles.AddUsersToRole(new string[] { userName }, roleName);
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Logging
                return false;
            }
        }

        public bool AddCurrentUserToRole(string roleName)
        {
            UserModel currentUser = this.GetCurrentUser();

            if (currentUser == null)
                return false;

            try
            {
                Roles.AddUsersToRole(new string[] { currentUser.Email }, roleName);
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Logging
                return false;
            }
        }

        public bool AddRoleToContentRights(Guid contentId, int roleId, int type = 1, int level = 0)
        {
            try
            {
                db.ContentRights.Add(new ContentRight
                    {
                        Id = Guid.NewGuid(),
                        ContentId = contentId,
                        Type = type,
                        RoleId = roleId,
                        Level = level
                    });

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Logging
                return false;
            }
        }

        public bool DeleteRole(string roleName)
        {
            string[] usersInRole = Roles.GetUsersInRole(roleName);
            if(usersInRole.Length > 0)
                Roles.RemoveUsersFromRole(usersInRole, roleName);
            return Roles.DeleteRole(roleName);
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

        public bool DeleteContentRights(Guid contentGuid)
        {
            try
            {
                this.db.Database.ExecuteSqlCommand("DELETE FROM ContentRights WHERE id = {0}", contentGuid.ToString());
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Logging
                return false;
            }
        }

        public int[] GetRoleIdsForUser(int userId)
        {
            return db.Users.FirstOrDefault(u => u.Id == userId).webpages_Roles.Select(r => r.RoleId).ToArray();
        }

        public bool UserIsAuthorized(IContent content)
        {
            if (!WebSecurity.IsAuthenticated)
                return false;

            int userId = WebSecurity.CurrentUserId;
            IEnumerable<int> roleIds;

            using (UserRepository users = new UserRepository())
            {
                roleIds = users.GetRoleIdsForUser(userId).AsEnumerable();
            }

            return content.ContentRights.Any(r => (r.Type == 1 && roleIds.Contains(r.RoleId)) || (r.Type == 2 && r.RoleId == userId));

        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
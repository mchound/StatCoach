using StatCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
using System.Web.Mvc;
using StatCoach.Business.Enums;

namespace StatCoach.Data
{
    public class StatsRepository : IDisposable
    {
        private dbStatCoachEntitiesAzure db;
        private System.Web.Caching.Cache cache;

        public StatsRepository()
        {
            this.db = new dbStatCoachEntitiesAzure();
            this.cache = new System.Web.Caching.Cache();
        }

        public List<SelectListItem> GetClubListItems()
        {
            if (HttpContext.Current.Cache["ClucListItems"] == null)
            {
                HttpContext.Current.Cache["ClucListItems"] = this.db.Clubs.AsEnumerable<Club>().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList<SelectListItem>();                
            }

            return (List<SelectListItem>)HttpContext.Current.Cache["ClucListItems"];
        }

        public List<TeamModel> GetTeamsByCurrentUser()
        {
            UserRepository users = new UserRepository();
            UserModel currentUser = users.GetCurrentUser();
            if(currentUser == null)
                return new List<TeamModel>();

            int userId = currentUser.Id;
            List<int> roleIds = users.GetRoleIdsForUser(userId).ToList();
            roleIds.Add(userId);

            var teams = db.Teams.Where(team => db.ContentRights.
                Where(right => roleIds.Any(roleId => roleId == right.RoleId) && db.Teams.Any(team2 => team2.Id == right.ContentId)).
                Any(r => r.ContentId == team.Id)).
                Select(t => new TeamModel 
                { 
                    Id = t.Id,
                    Name = t.Name,
                    CreatedByUserId = t.CreatedByUserId
                });

            return teams.ToList();
        }

        public CRUDStatus CreateTeam(string teamName)
        {
            UserRepository users = new UserRepository();
            UserModel currentUser = users.GetCurrentUser();

            if (currentUser == null)
                return CRUDStatus.UserError;

            Team team = new Team
            {
                Id = Guid.NewGuid(),
                Name = teamName,
                CreatedByUserId = currentUser.Id
            };

            int roleId;

            // Add role named as the team
            if ((roleId = users.AddRole(team.Name)) == -1)
                return CRUDStatus.RoleError;

            // Add current user to created role
            if (!users.AddUserToRole(currentUser.Email, team.Name))
            {
                users.DeleteRole(team.Name);
                return CRUDStatus.UserError;
            }

            // Add access rights for the team to the new role
            if (!users.AddRoleToContentRights(team.Id, roleId))
                return CRUDStatus.RoleError;

            team.RootRoleName = team.Name;
            db.Teams.Add(team);

            try
            {
                db.SaveChanges();
                users.Dispose();
            }
            catch (Exception ex)
            {
                // TODO: Logging
                return CRUDStatus.Fail;
            }

            return CRUDStatus.Success;

        }

        public CRUDStatus DeleteTeam(string teamId)
        {
            Guid _teamId = Guid.Parse(teamId);
            Team team = db.Teams.FirstOrDefault(t => t.Id == _teamId);
            if (team == null)
                return CRUDStatus.EntityNotFound;

            UserRepository users = new UserRepository();

            // Delete root role for team
            if (!users.DeleteRole(team.Name))
                return CRUDStatus.RoleError;

            // Delete access rights for team
            if (!users.DeleteContentRights(team.Id))
                return CRUDStatus.AccessRightsError;

            try
            {
                db.Teams.Remove(team);
                db.SaveChanges();
            }
            catch (Exception)
            {
                // TODO: Logging
                return CRUDStatus.Fail;
            }

            users.Dispose();
            return CRUDStatus.Success;

        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
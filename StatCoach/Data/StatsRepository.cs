using StatCoach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
using System.Web.Mvc;
using StatCoach.Business.Enums;
using StatCoach.Business.Extensions;
using StatCoach.Business.Interfaces;

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

        public List<Club> GetClubs()
        {
            if (HttpContext.Current.Cache["AllClubs"] == null)
            {
                HttpContext.Current.Cache["AllClubs"] = this.db.Clubs.ToList();
            }

            return (List<Club>)HttpContext.Current.Cache["AllClubs"];            
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

        public Club GetClubBySEOName(string ClubSEOName)
        {
            string clubName = ClubSEOName.FromSEO();
            return this.db.Clubs.FirstOrDefault(c => c.Name.ToLower() == clubName.ToLower());
        }

        public List<TeamModel> GetTeamsByClubId(Guid clubId)
        {
            return this.db.Teams.Where(t => t.ClubId == clubId).Select(t => new TeamModel
            {
                Id = t.Id,
                CreatedByUserId = t.CreatedByUserId,
                Name = t.Name
            }).ToList();
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

        public TeamModel GetTeamById(Guid teamId)
        {
            return this.db.Teams.Where(t => t.Id == teamId).Select(t => new TeamModel
            {
                CreatedByUserId = t.CreatedByUserId,
                Id = t.Id,
                Name = t.Name,
                ContentRights = db.ContentRights.Where(cr => cr.ContentId == t.Id)
            }).FirstOrDefault();
        }

        public IContent GetContentFromRoute(Guid clubId, string urlName)
        {
            Routes route = this.db.Routes1.FirstOrDefault(r => r.ClubId == clubId && r.URL.ToLower() == urlName);

            IContent content = null;
            switch ((ContentType)route.Type)
            {
                case ContentType.Club:
                    break;
                case ContentType.Team:
                    content = this.GetTeamById(route.ContentId);
                    content.Type = (ContentType)route.Type;
                    break;
                case ContentType.Player:
                    break;
                default:
                    break;
            }

            return content;
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
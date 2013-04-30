using StatCoach.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatCoach.Models
{
    public class UsersModel
    {
        public List<UserModel> Users;

        public UsersModel()
        {
            using (UserRepository users = new UserRepository())
            {
                this.Users = users.GetAllUsers().ToList();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;

namespace GCBA.Repositories
{
    public class UserDataAccess
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public List<ApplicationUser> GetAll()
        {
            var result = dbContext.Users.ToList();

            return result;
        }

        public ApplicationUser GetByUsername(string username)
        {
            var result = dbContext.Users.SingleOrDefault(c => c.UserName.ToLower() == username.ToLower());
            return result;
        }

        public ApplicationUser GetByEmail(string email)
        {
            var result = dbContext.Users.SingleOrDefault(c => c.Email.ToLower() == email.ToLower());
            return result;
        }


    }
}
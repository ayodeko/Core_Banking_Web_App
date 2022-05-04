using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;
using GCBA.Repositories;

namespace GCBA.Logic
{
    public class TellerLogic
    {
        UserDataAccess userData = new UserDataAccess();
        public List<ApplicationUser> GetAllTellers()
        {
            var result = userData.GetAll().Where(c => c.Role.Name == "Teller").ToList();

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;

namespace GCBA.Repositories
{
    public class TellerDataAccess
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public List<ApplicationUser> GetAllTellers()
        {
            var result = dbContext.Users.Where(c => c.Role.Name == "Teller").ToList();
            return result;
        }

        public List<ApplicationUser> GetTellersWithNoTills()
        {
            var tellers = GetAllTellers();
            var tillAccounts = dbContext.TillAccounts.ToList();
            var result = new List<ApplicationUser>();

            foreach (var teller in tellers)
            {
                if (!tillAccounts.Any(c => c.UserId == teller.Id))
                {
                    result.Add(teller);
                }
            }


            return result;
        }

        private GlAccountDataAccess glDataAccess = new GlAccountDataAccess();
        public List<TillAccount> GetAllTellerDetails()
        {
            var output = new List<TillAccount>();
            var tillsWithTellers = GetDbTillAccounts();
            var tellersWithoutTill = GetTellersWithNoTills();

            //adding all tellers without a till account
            foreach (var teller in tellersWithoutTill)
            {
                output.Add(new TillAccount { UserId = teller.Id, GlAccountID = 0 });
            }
            //adding all tellers with a till account
            output.AddRange(tillsWithTellers);
            return output;
        }

        public List<TillAccount> GetDbTillAccounts()
        {
            return dbContext.TillAccounts.ToList();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;

namespace GCBA.Repositories
{
    public class GlAccountDataAccess
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public List<GlAccount> GetAll()
        {
            var glAccountList = dbContext.GlAccounts.ToList();
            return glAccountList;
        }

        public bool IsGlCategoryIsDeletable(int id)
        {
            return GetAll().Any(c => c.GlCategoryID == id);
        }

        public GlAccount GetLastGlIn(MainGlCategory mainCategory)
        {
            return dbContext.GlAccounts.Where(g => g.GlCategory.MainGlCategory == mainCategory).OrderByDescending(a => a.ID).First();
        }

        public bool AnyGlIn(MainGlCategory mainCategory)
        {
            return dbContext.GlAccounts.Any(gl => gl.GlCategory.MainGlCategory == mainCategory);
        }
        public GlAccount GetByName(string Name)
        {
            var glAccountByName = dbContext.GlAccounts.SingleOrDefault(c => c.AccountName == Name);
            return glAccountByName;
        }

        public GlAccount GetById(int Id)
        {
            var glAccount = dbContext.GlAccounts.SingleOrDefault(c => c.ID == Id);
            return glAccount;
        }

        public List<GlAccount> GetAllTills()
        {
            var tills = dbContext.GlAccounts.Where(c => c.AccountName.ToLower().Contains("till")).ToList();
            return tills;
        }

        public List<GlAccount> GetTillsWithoutTellers()
        {
            var output = new List<GlAccount>();
            var allTills = GetAllTills();
            var tillAccount = dbContext.TillAccounts.ToList();

            foreach (var till in allTills)
            {
                if (tillAccount.Any(c => c.GlAccountID == till.ID))
                {
                    output.Add(till);
                }
            }

            return output;
        }

        public List<GlAccount> GetAllAssetAccounts()
        {
            var output = dbContext.GlAccounts.Where(c => c.GlCategory.MainGlCategory == MainGlCategory.Asset).ToList();

            return output;
        }

        public List<GlAccount> GetAllIncomeAccounts()
        {
            var output = dbContext.GlAccounts.Where(c => c.GlCategory.MainGlCategory == MainGlCategory.Income).ToList();

            return output;
        }

        public List<GlAccount> GetAllLiabilityAccounts()
        {
            var output = dbContext.GlAccounts.Where(c => c.GlCategory.MainGlCategory == MainGlCategory.Liability).ToList();

            return output;
        }

        public List<GlAccount> GetAllExpenseAccounts()
        {
            var output = dbContext.GlAccounts.Where(c => c.GlCategory.MainGlCategory == MainGlCategory.Expense).ToList();

            return output;
        }

        public List<GlAccount> GetByMainCategory(MainGlCategory mainCategory)
        {
            return dbContext.GlAccounts.Where(a => a.GlCategory.MainGlCategory == mainCategory).ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;

namespace GCBA.Repositories
{
    public class AccountTypeManagementRepository
    {

        ApplicationDbContext dbContext = new ApplicationDbContext();

        public AccountTypeManagement GetFirst()
        {
            return dbContext.AccountTypeManagements.First();
        }
    }
}
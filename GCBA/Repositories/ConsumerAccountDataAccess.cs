using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;

namespace GCBA.Repositories
{
    public class ConsumerAccountDataAccess
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<ConsumerAccount> GetByType(AccountType actType)
        {
            //using (ISession session = NHibernateHelper.OpenSession())
            //{
            //    return session.Query<CustomerAccount>().Where(a => a.AccountType == actType).ToList();
            //}
            return db.ConsumerAccounts.Where(a => a.AccountType == actType).ToList();
        }

        public int GetCountByCustomerActType(AccountType actType, int customerId)
        {
            return db.ConsumerAccounts.Where(a => a.AccountType == actType && a.Consumer.ID == customerId).Count();
        }

        public bool AnyAccountOfType(AccountType type)
        {
            //using (ISession session = NHibernateHelper.OpenSession())
            //{
            //    return session.Query<CustomerAccount>().Any(a => a.AccountType == type);
            //}
            return db.ConsumerAccounts.Any(a => a.AccountType == type);
        }
    }
}
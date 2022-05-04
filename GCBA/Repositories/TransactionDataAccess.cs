using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;

namespace GCBA.Repositories
{
    public class TransactionDataAccess
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public List<Transaction> GetTrialBalanceTransactions(DateTime startDate, DateTime endDate)
        {
            var result = new List<Transaction>();
            if (startDate < endDate)
            {
                var allTransactions = db.Transactions.ToList();
                foreach (var item in allTransactions)
                {
                    if (item.Date.Date >= startDate && item.Date.Date <= endDate)
                    {
                        result.Add(item);
                    }
                }

            }
            return result;
        }
    }
}
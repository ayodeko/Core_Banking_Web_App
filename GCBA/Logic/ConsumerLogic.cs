using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Data;

namespace GCBA.Logic
{
    public class ConsumerLogic
    {
        private ConsumerDataAccess _consumerDataAccess;
        public string GenerateCustomerLongId()
        {

            _consumerDataAccess = new ConsumerDataAccess();

            string id = "00000001";

            var customers = _consumerDataAccess.GetAll().OrderByDescending(c => c.ID);

            if (customers.Any())
            {
                long newId = Convert.ToInt64(customers.First().ConsumerLongID);
                id = (newId + 1).ToString("D8");
            }

            return id;
        }
    }
}
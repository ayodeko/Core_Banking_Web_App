using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GCBA.Models;
using Microsoft.Owin.Security.Provider;

namespace GCBA.Data
{
    public  class ConsumerDataAccess
    {
        public ApplicationDbContext Context = new ApplicationDbContext();
        public Consumer GetById(int id)
        {
            Consumer result = Context.Consumers.SingleOrDefault(c => c.ID.Equals(id));
            return result;
        }

        public List<Consumer> GetAll()
        {
            return Context.Consumers.ToList();
        }


        
    }
}
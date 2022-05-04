using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;

namespace GCBA.Repositories
{
    public class GlCategoriesDataAccess
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public GlCategory GetByName(string categoryName)
        {
            GlCategory result = dbContext.GlCategories.SingleOrDefault(c => c.Name == categoryName);
            return result;
        }

        public GlCategory GetById(int Id)
        {
            var glCategory = dbContext.GlCategories.SingleOrDefault(c => c.ID == Id);
            return glCategory;
        }

    }
}
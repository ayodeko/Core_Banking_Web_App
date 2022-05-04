using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;
using GCBA.Repositories;

namespace GCBA.Logic
{
    public class GlAccountLogic
    {
        GlAccountDataAccess glAccountDataAccess = new GlAccountDataAccess();

        GlCategoriesDataAccess glCategoriesDataAccess = new GlCategoriesDataAccess();

        ApplicationDbContext dbContext = new ApplicationDbContext();

        public bool IsUnique(string glAccountName)
        {
            if (glAccountDataAccess.GetByName(glAccountName) == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        GlAccountDataAccess glRepo = new GlAccountDataAccess();
        public long GenerateGLAccountNumber(MainGlCategory glMainCategory)
        {
            long code = 0;

            //get the last account number in this category
            if (glRepo.AnyGlIn(glMainCategory))
            {
                var lastAct = glRepo.GetLastGlIn(glMainCategory);
                code = lastAct.Code + 1;
            }

            else                //this is going to be the first act in this category
            {
                switch (glMainCategory)     //these codes are assumed at author's descretion
                {
                    case MainGlCategory.Asset:
                        code = 10001020;
                        break;
                    case MainGlCategory.Capital:
                        code = 30001020;
                        break;
                    case MainGlCategory.Expense:
                        code = 50001020;
                        break;
                    case MainGlCategory.Income:
                        code = 40001020;
                        break;
                    case MainGlCategory.Liability:
                        code = 20001020;
                        break;
                    default:
                        break;
                }
            }//end if

            return code;
        }
    }
}
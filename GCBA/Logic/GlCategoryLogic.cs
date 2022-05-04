using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;
using GCBA.Repositories;

namespace GCBA.Logic
{
    public class GlCategoryLogic
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        GlCategoriesDataAccess dlCatDataAccess = new GlCategoriesDataAccess();
        public bool isUnique(string glName)
        {
            if (dlCatDataAccess.GetByName(glName) == null)
            {
                return true;
            }

            return false;
        }

        public long CreateGlCategoryCode(GlCategory glCategory)
        {
            long newGlCode = 10;
            MainGlCategory mainGl = glCategory.MainGlCategory;

            var categoryList = dbContext.GlCategories.ToList().OrderByDescending(c=>c.ID);

            if (categoryList.Any())
            {
                var lastGlCode = categoryList.First().Code;
                var stringLastGlCode = lastGlCode.ToString();
                /*Get the main GlCode*/
                int endIndex = stringLastGlCode.Length - 3;
                string mainGlCode = stringLastGlCode.Substring(3, endIndex);

                lastGlCode = Convert.ToInt64(mainGlCode);

                newGlCode = lastGlCode + 10;
                
            }

            string stringGlCode = newGlCode.ToString();
            long finalGlCode;

            switch (mainGl)
            {
                case MainGlCategory.Asset:
                    finalGlCode = Convert.ToInt64(MainCategoryCodes.AssetCode + stringGlCode);
                    break;
                case MainGlCategory.Capital:
                    finalGlCode = Convert.ToInt64(MainCategoryCodes.CapitalCode + stringGlCode);
                    break;
                case MainGlCategory.Expense:
                    finalGlCode = Convert.ToInt64(MainCategoryCodes.ExpenseCode + stringGlCode);
                    break;
                case MainGlCategory.Income:
                    finalGlCode = Convert.ToInt64(MainCategoryCodes.IncomeCode + stringGlCode);
                    break;
                case MainGlCategory.Liability:
                    finalGlCode = Convert.ToInt64(MainCategoryCodes.LiabilityCode + stringGlCode);
                    break;
                default:
                    finalGlCode = 000;
                    break;
            }

            return finalGlCode;
        }

        
    }

    public class MainCategoryCodes
    {
        public static string AssetCode = "100";
        public static string LiabilityCode = "200";
        public static string CapitalCode = "300";
        public static string IncomeCode = "400";
        public static string ExpenseCode = "500";
    }
}
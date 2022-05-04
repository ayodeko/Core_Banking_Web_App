using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models.ViewModels;
using GCBA.Repositories;

namespace GCBA.Models
{
    public class BalanceSheetLogic
    {
        BalanceSheetDataAccess bsRepo = new BalanceSheetDataAccess();

        public List<GlAccount> GetAssetAccounts()
        {
            return bsRepo.GetAssetAccounts();
        }

        public List<GlAccount> GetCapitalAccounts()
        {
            return bsRepo.GetCapitalAccounts();
        }

        public List<LiabilityViewModel> GetLiabilityAccounts()
        {
            return bsRepo.GetLiabilityAccounts();
        }
    }
}
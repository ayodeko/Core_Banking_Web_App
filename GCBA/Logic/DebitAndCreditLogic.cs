using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;
using GCBA.Repositories;

namespace GCBA.Logic
{
    public class DebitAndCreditLogic
    {

        AccountTypeManagementRepository configRepo = new AccountTypeManagementRepository();
        public bool IsConfigurationSet()
        {
            var config = configRepo.GetFirst();
            if (config.SavingsInterestExpenseGl == null || config.SavingsInterestPayableGl == null || config.CurrentInterestExpenseGl == null || config.COTIncomeGl == null || config.LoanInterestIncomeGl == null || config.LoanInterestReceivableGl == null)
            {
                // config.LoanInterestExpenseGl == null || 
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CreditConsumerAccount(ConsumerAccount consumerAccount, decimal amount)
        {

            try
            {
                if (consumerAccount.AccountType == AccountType.Loan)
                {
                    consumerAccount.AccountBalance -= amount;    //Because a Loan Account is an Asset Account
                }
                else
                {
                    consumerAccount.AccountBalance += amount;     //Because a Savings or Current Account is a Liability Account
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool DebitConsumerAccount(ConsumerAccount consumerAccount, decimal amount)
        {

            try
            {
                if (consumerAccount.AccountType == AccountType.Loan)
                {
                    consumerAccount.AccountBalance += amount;    //Because a Loan Account is an Asset Account
                }
                else
                {
                    consumerAccount.AccountBalance -= amount;     //Because a Savings or Current Account is a Liability Account
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool CreditGl(GlAccount account, decimal amount)
        {
            try
            {
                switch (account.GlCategory.MainGlCategory)
                {
                    case MainGlCategory.Asset:
                        account.AccountBalance -= amount;
                        break;
                    case MainGlCategory.Capital:
                        account.AccountBalance += amount;
                        break;
                    case MainGlCategory.Expense:
                        account.AccountBalance -= amount;
                        break;
                    case MainGlCategory.Income:
                        account.AccountBalance += amount;
                        break;
                    case MainGlCategory.Liability:
                        account.AccountBalance += amount;
                        break;
                    default:
                        break;
                }//end switch

                //frLogic.CreateTransaction(account, amount, TransactionType.Credit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }//end creditGl

        public bool DebitGl(GlAccount account, decimal amount)
        {
            try
            {
                switch (account.GlCategory.MainGlCategory)
                {
                    case MainGlCategory.Asset:
                        account.AccountBalance += amount;
                        break;
                    case MainGlCategory.Capital:
                        account.AccountBalance -= amount;
                        break;
                    case MainGlCategory.Expense:
                        account.AccountBalance += amount;
                        break;
                    case MainGlCategory.Income:
                        account.AccountBalance -= amount;
                        break;
                    case MainGlCategory.Liability:
                        account.AccountBalance -= amount;
                        break;
                    default:
                        break;
                }//end switch
                //frLogic.CreateTransaction(account, amount, TransactionType.Debit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }//end DebitGl

    }
}
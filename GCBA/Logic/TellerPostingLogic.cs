using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Models;
using GCBA.Repositories;

namespace GCBA.Logic
{
    public class TellerPostingLogic
    {


        DebitAndCreditLogic busLogic = new DebitAndCreditLogic();
        public string PostTeller(ConsumerAccount account, GlAccount till, decimal amt, TellerPostingType pType)
        {
            string output = "";
            switch (pType)
            {
                case TellerPostingType.Deposit:
                    busLogic.CreditConsumerAccount(account, amt);
                    busLogic.DebitGl(till, amt);

                    output = "success";
                    break;
                //break;
                case TellerPostingType.Withdrawal:
                    //Transfer the money from the user's till and reflect the changes in the customer account balance
                    //check withdrawal limit

                    var config = new AccountTypeManagementRepository().GetFirst();
                    //till = user.TillAccount;
                    if (account.AccountType == AccountType.Savings)
                    {
                        if (account.AccountBalance >= config.SavingsMinimumBalance + amt)
                        {
                            if (till.AccountBalance >= amt)
                            {
                                busLogic.CreditGl(till, amt);
                                busLogic.DebitConsumerAccount(account, amt);

                                output = "success";
                                account.SavingsWithdrawalCount++;
                            }
                            else
                            {
                                output = "Insufficient fund in the Till account";
                            }
                        }
                        else
                        {
                            output = "insufficient Balance in Customer's account, cannot withdraw!";
                        }

                    }//end if savings


                    else if (account.AccountType == AccountType.Current)
                    {
                        if (account.AccountBalance >= config.CurrentMinimumBalance + amt)
                        {
                            
                            if (till.AccountBalance >= amt)
                            {
                                busLogic.CreditGl(till, amt);
                                busLogic.DebitConsumerAccount(account, amt);

                                output = "success";
                                //decimal x = (amt + account.CurrentLien) / 1000;
                                decimal x = (amt) / 1000;
                                decimal charge = (int)x * config.COT;
                                account.dailyInterestAccrued += charge;
                                //account.CurrentLien = (x - (int)x) * 1000;
                            }
                            else
                            {
                                output = "Insufficient fund in the Till account";
                            }
                        }
                        else
                        {
                            output = "insufficient Balance in Customer's account, cannot withdraw!";
                        }

                    }
                    else //for loan
                    {
                        output = "Please select a valid account";
                    }
                    break;
                //break;
                default:
                    break;
            }//end switch
            return output;
        }//end method PostTeller
    }


}

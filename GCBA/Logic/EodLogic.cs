using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GCBA.Models;
using GCBA.Repositories;
using Microsoft.Ajax.Utilities;

namespace GCBA.Logic
{
    public class EodLogic
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //AccountTypeManagementRepository config = new AccountTypeManagementRepository();
        AccountTypeManagementRepository configRepo = new AccountTypeManagementRepository();
        ConsumerAccountDataAccess custActRepo = new ConsumerAccountDataAccess();
        GlAccountDataAccess glRepo = new GlAccountDataAccess();

        private DebitAndCreditLogic busLogic = new DebitAndCreditLogic();
        FinancialReportLogic frLogic = new FinancialReportLogic();

        AccountTypeManagement config;
        DateTime today;

        public EodLogic()
        {
            config = db.AccountTypeManagements.First();
            today = config.FinancialDate;
        }
        int[] daysInMonth = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        public string RunEOD()
        {
            string output = "";
            //check if all configurations are set
            try
            {
                if (busLogic.IsConfigurationSet())
                {
                    if (config.IsOpened == false)
                    {
                        CloseBusiness();
                        ComputeSavingsInterestAccrued();
                        ComputeCurrentInterestAccrued();
                        ComputeLoanInterestAccrued();
                        SaveDailyIncomeAndExpenseBalance(); //to calculate Profit and Loss

                        //var config = db.AccountConfiguration.First();
                        config.FinancialDate =
                            config.FinancialDate.AddDays(1); //increments the financial date at the EOD
                        output = "Successfully Run EOD!";
                        db.Entry(config).State = EntityState.Modified;
                        db.SaveChanges();
                        //Ensures all or none of the 5 steps above executes and gets saved                     
                        
                    }
                    else
                    {
                        config.IsOpened = true;
                        output = "Business Opened";
                    }
                }
                else
                {
                    output = "Configurations not set!";
                }
            }
            catch (Exception)
            {
                output = "An error occured while running EOD";
            }
            
            return output;
        }
        public void CloseBusiness()
        {
            //var config = db.AccountConfiguration.First();
            config.IsOpened = false;
            db.Entry(config).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void OpenBusiness()
        {
            //var config = db.AccountConfiguration.First();
            config.IsOpened = true;
            db.Entry(config).State = EntityState.Modified;
            db.SaveChanges();
        }
        public bool isBusinessClosed()
        {
            //var config = new ConfigurationRepository().GetFirst();
            if (config.IsOpened)
            {
                return false;
            }
            else
            {
                return true;
            }
        }//

        public void ComputeSavingsInterestAccrued()
        {
            //var config = db.AccountConfiguration.First();
            //DateTime today = DateTime.Now;
            int presentDay = today.Day;     //1 to totalDays in d month
            int presentMonth = today.Month;     //1 to 12
            int daysRemaining = 0;
            if (custActRepo.AnyAccountOfType(AccountType.Savings))
            {
                //var allSavings = custActRepo.GetByType(AccountType.Savings);
                var allSavings = db.ConsumerAccounts.Where(a => a.AccountType == AccountType.Savings).ToList();
                //foreach savings account
                foreach (var account in allSavings)
                {
                    //get the no of days remaining in this month
                    daysRemaining = daysInMonth[presentMonth - 1] - presentDay + 1;     //+1 because we havent computed for today
                    decimal interestRemainingForTheMonth = account.AccountBalance * (decimal)config.SavingsCreditInterestRate * daysRemaining / daysInMonth[presentMonth - 1];      //using I = PRT, where R is per month
                    //calculate today's interest and add it to the account's dailyInterestAccrued
                    decimal todaysInterest = interestRemainingForTheMonth / daysRemaining;
                    account.dailyInterestAccrued += todaysInterest;     //increments till month end. Disbursed if withdrawal limit is not exceeded

                    busLogic.DebitGl(config.SavingsInterestExpenseGl, todaysInterest);
                    busLogic.CreditGl(config.SavingsInterestPayableGl, todaysInterest);

                    db.Entry(account).State = EntityState.Modified;
                    db.Entry(config.SavingsInterestExpenseGl).State = EntityState.Modified;
                    db.Entry(config.SavingsInterestPayableGl).State = EntityState.Modified;

                    db.SaveChanges();
                    //custActRepo.Update(account);
                    //glRepo.Update(config.SavingsInterestExpenseGl);
                    //glRepo.Update(config.SavingsInterestPayableGl);

                    frLogic.CreateTransaction(config.SavingsInterestExpenseGl, todaysInterest, TransactionType.Debit);
                    frLogic.CreateTransaction(config.SavingsInterestPayableGl, todaysInterest, TransactionType.Credit);
                }//end foreach

                //monthly savings interest payment
                if (today.Day == daysInMonth[presentMonth - 1])     //MONTH END?
                {
                    bool customerIsCredited = false;
                    foreach (var account in allSavings)
                    {
                        
                        busLogic.DebitGl(config.SavingsInterestPayableGl, Convert.ToDecimal(account.dailyInterestAccrued));

                        //if the Withdrawal limit is not exceeded
                        if (!(account.SavingsWithdrawalCount > 3))    //assuming the withdrawal limit is 3
                        {
                            //Credit the customer ammount
                            busLogic.CreditConsumerAccount(account, Convert.ToDecimal(account.dailyInterestAccrued));
                            customerIsCredited = true;
                        }
                        else
                        {
                            busLogic.CreditGl(config.SavingsInterestExpenseGl, Convert.ToDecimal(account.dailyInterestAccrued));
                        }
                        account.SavingsWithdrawalCount = 0;  //re-initialize to zero for the next month
                        account.dailyInterestAccrued = 0;

                        db.Entry(account).State = EntityState.Modified;
                        db.Entry(config.SavingsInterestExpenseGl).State = EntityState.Modified;
                        db.Entry(config.SavingsInterestPayableGl).State = EntityState.Modified;

                        db.SaveChanges();
                        

                        frLogic.CreateTransaction(config.SavingsInterestPayableGl, Convert.ToDecimal(account.dailyInterestAccrued), TransactionType.Debit);
                        if (customerIsCredited)
                        {
                            frLogic.CreateTransaction(account, Convert.ToDecimal(account.dailyInterestAccrued), TransactionType.Credit);
                        }
                        frLogic.CreateTransaction(config.SavingsInterestExpenseGl, Convert.ToDecimal(account.dailyInterestAccrued), TransactionType.Credit);
                    }
                }//end if
                
            }
        }//end method ComputeAllSavingsInterestAccrued

        public void ComputeCurrentInterestAccrued()
        {
            if (custActRepo.AnyAccountOfType(AccountType.Current))
            {
                //note that COT is calculated upon withdarawal and not on a daily basis
                //the accrued COT is then deducted at month end
                int presentMonth = today.Month;     //1 to 12
                //monthly COT deduction
                if (today.Day == daysInMonth[presentMonth - 1])     //MONTH END?
                {
                    //var allCurrents = custActRepo.GetByType(AccountType.Current);
                    var allCurrents = db.ConsumerAccounts.Where(a => a.AccountType == AccountType.Current).ToList();

                    foreach (var currentAccount in allCurrents)
                    {
                        //Debit customer account
                        //currentAccount.AccountBalance -= currentAccount.dailyInterestAccrued;   //accrued COT
                        busLogic.DebitConsumerAccount(currentAccount, Convert.ToDecimal(currentAccount.dailyInterestAccrued));
                        busLogic.CreditGl(config.COTIncomeGl, Convert.ToDecimal(currentAccount.dailyInterestAccrued));

                        currentAccount.dailyInterestAccrued = 0;    //goes back to zero after monthly deduction

                        db.Entry(currentAccount).State = EntityState.Modified;
                        db.Entry(config.COTIncomeGl).State = EntityState.Modified;

                        db.SaveChanges();
                        //custActRepo.Update(currentAccount);
                        //glRepo.Update(config.CurrentCotIncomeGl);

                        frLogic.CreateTransaction(currentAccount, Convert.ToDecimal(currentAccount.dailyInterestAccrued), TransactionType.Debit);
                        frLogic.CreateTransaction(config.COTIncomeGl, Convert.ToDecimal(currentAccount.dailyInterestAccrued), TransactionType.Credit);
                    }
                    //configRepo.Update(config);
                }//end if
                //db.SaveChanges();
            }//end if    
        }//end current compute

        public void ComputeLoanInterestAccrued()
        {
            int presentMonth = today.Month;     //1 to 12
            decimal dailyInterestRepay = 0;

            if (custActRepo.AnyAccountOfType(AccountType.Loan))
            {
                //var allLoans = custActRepo.GetByType(AccountType.Loan);
                var allLoans = db.ConsumerAccounts.Where(a => a.AccountType == AccountType.Loan).ToList();
                //daily loan repay
                foreach (var loanAccount in allLoans)
                {
                    dailyInterestRepay = loanAccount.LoanMonthlyInterestRepay / 30;     //assume 30 days in a month
                    loanAccount.dailyInterestAccrued += dailyInterestRepay;

                    busLogic.DebitGl(config.LoanInterestReceivableGl, dailyInterestRepay);
                    busLogic.CreditGl(config.LoanInterestIncomeGl, dailyInterestRepay);

                    loanAccount.DaysCount++;  //increments the day account was created after every EOD

                    db.Entry(loanAccount).State = EntityState.Modified;
                    db.Entry(config.LoanInterestReceivableGl).State = EntityState.Modified;
                    db.Entry(config.LoanInterestIncomeGl).State = EntityState.Modified;

                    db.SaveChanges();
                    //custActRepo.Update(loanAccount);
                    //configRepo.Update(config);
                    //glRepo.Update(config.LoanInterestReceivableGl);
                    //glRepo.Update(config.LoanInterestIncomeGl);

                    frLogic.CreateTransaction(config.LoanInterestReceivableGl, dailyInterestRepay, TransactionType.Debit);
                    frLogic.CreateTransaction(config.LoanInterestIncomeGl, dailyInterestRepay, TransactionType.Credit);
                }//end foreach

                //monthly loan deduction
                foreach (var loanAccount in allLoans)
                {
                    if (loanAccount.DaysCount % 30 == 0)      //checks monthly (30 days) cycle
                    {
                        //pay back interest
                        busLogic.CreditGl(config.LoanInterestReceivableGl, Convert.ToDecimal(loanAccount.dailyInterestAccrued));    //so the interestReceivable account balance goes back to zero                      
                        busLogic.DebitConsumerAccount(loanAccount.LinkedAccount, Convert.ToDecimal(loanAccount.dailyInterestAccrued));

                        db.Entry(config.LoanInterestReceivableGl).State = EntityState.Modified;
                        db.Entry(loanAccount.LinkedAccount).State = EntityState.Modified;

                        db.SaveChanges();
                        //glRepo.Update(config.LoanInterestReceivableGl);
                        //custActRepo.Update(loanAccount.ServicingAccount);

                        frLogic.CreateTransaction(config.LoanInterestReceivableGl, Convert.ToDecimal(loanAccount.dailyInterestAccrued), TransactionType.Credit);
                        frLogic.CreateTransaction(loanAccount.LinkedAccount, Convert.ToDecimal(loanAccount.dailyInterestAccrued), TransactionType.Debit);

                        loanAccount.dailyInterestAccrued = 0;       //returns to zero after it has been debitted

                        //pay back monthly principal
                        busLogic.CreditConsumerAccount(loanAccount, loanAccount.LoanMonthlyPrincipalRepay);
                        busLogic.DebitConsumerAccount(loanAccount.LinkedAccount, loanAccount.LoanMonthlyPrincipalRepay);

                        db.Entry(loanAccount).State = EntityState.Modified;
                        db.Entry(loanAccount.LinkedAccount).State = EntityState.Modified;

                        db.SaveChanges();
                        //custActRepo.Update(loanAccount);
                        //custActRepo.Update(loanAccount.ServicingAccount);

                        frLogic.CreateTransaction(loanAccount, loanAccount.LoanMonthlyPrincipalRepay, TransactionType.Credit);
                        frLogic.CreateTransaction(loanAccount.LinkedAccount, loanAccount.LoanMonthlyPrincipalRepay, TransactionType.Debit);

                        if (loanAccount.TermsOfLoan == TermsOfLoan.Reducing)        //the monthly paymment will change
                        {
                            loanAccount.LoanMonthlyInterestRepay = Convert.ToDecimal(loanAccount.LoanInterestRatePerMonth * loanAccount.LoanPrincipalRemaining);
                            loanAccount.LoanMonthlyPrincipalRepay = loanAccount.LoanMonthlyRepay - loanAccount.LoanMonthlyInterestRepay;
                            loanAccount.LoanPrincipalRemaining = loanAccount.LoanMonthlyPrincipalRepay;
                        }

                        db.Entry(loanAccount).State = EntityState.Modified;
                        db.SaveChanges();
                        //custActRepo.Update(loanAccount);
                        //configRepo.Update(config);
                    }
                }
                //db.SaveChanges();
            }//end if         
        }//end method ComputeDailyLoanInterestAccrued

        public void SaveDailyIncomeAndExpenseBalance()
        {
            var allIncomes = glRepo.GetByMainCategory(MainGlCategory.Income);
            //save daily balance of all income
            foreach (var account in allIncomes)
            {
                var entry = new ExpenseIncomeEntry();
                entry.AccountName = account.AccountName;
                entry.Amount = account.AccountBalance;
                entry.Date = today;
                entry.EntryType = PandLType.Income;
                db.ExpenseIncomeEntries.Add(entry);
                db.SaveChanges();
                //new ProfitAndLossRepository().Insert(entry);
            }

            //save daily balance off all expense accounts
            var allExpenses = glRepo.GetByMainCategory(MainGlCategory.Expense);
            foreach (var account in allExpenses)
            {
                var entry = new ExpenseIncomeEntry();
                entry.AccountName = account.AccountName;
                entry.Amount = account.AccountBalance;
                entry.Date = today;
                entry.EntryType = PandLType.Expenses;
                db.ExpenseIncomeEntries.Add(entry);
                db.SaveChanges();
                //new ProfitAndLossRepository().Insert(entry);
            }
        }
    }
}
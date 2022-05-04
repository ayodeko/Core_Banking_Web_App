using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCBA.Data;
using GCBA.Models;
using GCBA.Repositories;
using Microsoft.Ajax.Utilities;

namespace GCBA.Logic
{
    public class ConsumerAccountLogic
    {


        ConsumerDataAccess _customerDataAccess = new ConsumerDataAccess();
        AccountTypeManagementRepository accountTypeRepo = new AccountTypeManagementRepository();

        public string CreateAccountNumber(AccountType accountType, ConsumerAccount consumerAccount)
        {
            //int customerId = customerAccount.CustomerID;

            int consumerId = consumerAccount.ConsumerID;
            Consumer consumer = _customerDataAccess.GetById(consumerId);

            if (consumer.ConsumerLongID.IsNullOrWhiteSpace())
            {
                return "";
            }

            long longId = Convert.ToInt64(consumer.ConsumerLongID);

            if (accountType == AccountType.Savings)
            {
                long accountNumber = AccountTypes.SavingsId + longId;
                return accountNumber.ToString();
            }

            if (accountType == AccountType.Current)
            {
                long accountNumber = AccountTypes.CurrentId + longId;
                return accountNumber.ToString();
            }
            if (accountType == AccountType.Loan)
            {
                long accountNumber = AccountTypes.LoanId + longId;
                return accountNumber.ToString();
            }

            return "";
        }

        public void ComputeFixedRepayment(ConsumerAccount act, double nyears, double interestRate)
        {
            decimal totalAmountToRepay = 0;
            double nMonth = nyears * 12;
            double totalInterest = interestRate * nMonth * (double)act.LoanAmount;
            totalAmountToRepay = (decimal)totalInterest + (decimal)act.LoanAmount;
            act.LoanMonthlyRepay = (totalAmountToRepay / (12 * (decimal)nyears));
            act.LoanMonthlyPrincipalRepay = Convert.ToDecimal((double)act.LoanAmount / nMonth);
            act.LoanMonthlyInterestRepay = Convert.ToDecimal(totalInterest / nMonth);
            act.LoanPrincipalRemaining = (decimal)act.LoanAmount;
        }

        public void ComputeReducingRepayment(ConsumerAccount act, double nyears, double interestRate)
        {
            double x = 1 - Math.Pow((1 + interestRate), -(nyears * 12));
            act.LoanMonthlyRepay = ((decimal)act.LoanAmount * (decimal)interestRate) / (decimal)x;

            act.LoanPrincipalRemaining = (decimal)act.LoanAmount;
            act.LoanMonthlyInterestRepay = (decimal)interestRate * act.LoanPrincipalRemaining;
            act.LoanMonthlyPrincipalRepay = act.LoanMonthlyRepay - act.LoanMonthlyInterestRepay;
        }

        public bool CheckIfAccountBalanceIsEnough(ConsumerAccount account, decimal amountToDebit)
        {
            var accountConfig = accountTypeRepo.GetFirst();

            if (account.AccountType == AccountType.Savings)
            {
                if (account.AccountBalance >= amountToDebit + accountConfig.SavingsMinimumBalance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (account.AccountType == AccountType.Current)
            {

                if (account.AccountBalance >= amountToDebit + accountConfig.CurrentMinimumBalance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }

    

    public static class AccountTypes
    {
        public static long SavingsId = 10000000;
        public static long CurrentId = 20000000;
        public static long LoanId = 30000000;

        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GCBA.Models
{

    public enum AccountStatus
    {
        Closed, Open
    }

    public enum TermsOfLoan
    {
        Fixed, Reducing
    }

    public enum AccountType
    {
        Savings, Current, Loan
    }


    public class ConsumerAccount
    {

        public int ID { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Enter Only Characters!")]
        public string AccountName { get; set; }


        [Display(Name = "Account Number")]
        [RegularExpression(@"^[0-9]*$")]
        public string AccountNumber { get; set; }

        [Display(Name = "Account Balance")]
        [DataType(DataType.Currency)]
        public decimal AccountBalance { get; set; }

        [Display(Name = "Branch Name")]
        public int BranchID { get; set; }

        [Display(Name = "Branch Name")]
        public virtual Branch Branch { get; set; }



        [Display(Name = "Account Status")]
        public AccountStatus AccountStatus { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }


        [Display(Name = "Date Created")]
        


        public int ConsumerID { get; set; }
        public virtual Consumer Consumer { get; set; }



        

        //Accounting Part

        [Display(Name = "Virtual days count since loan account creation")]
        public int? DaysCount { get; set; }      //counts the number of days (at EOD) from account creation, (esp for loan accounts)

        [Display(Name = "Savings Interest Accrued / Current COT Accrued / Loan Interest Accrued")]
        public decimal? dailyInterestAccrued { get; set; }
        [Range(0, 100, ErrorMessage = "Interest rate must be between 0 an 100")]
        [Display(Name = "Loan Interest Rate Per Month")]
        public decimal? LoanInterestRatePerMonth { get; set; }
        [Display(Name = "Savings Withdrawal Count")]
        public int? SavingsWithdrawalCount { get; set; }

        [Display(Name = "Current Lien")]
        public decimal? CurrentLien { get; set; }       //holding amount


        



        //Creating the Loan Account

        [Display(Name = "Loan Monthly Interest Repay")]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal LoanMonthlyInterestRepay { get; set; }

        [Display(Name = "Loan Monthly Repay")]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal LoanMonthlyRepay { get; set; }

        [Display(Name = "Loan Monthly Principal Repay")]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal LoanMonthlyPrincipalRepay { get; set; }

        [Display(Name = "Loan Principal Remaining")]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal LoanPrincipalRemaining { get; set; }

        [Display(Name = "Terms Of Loan")]
        public TermsOfLoan? TermsOfLoan { get; set; }

        /*[Required(ErrorMessage = "Number of years is required")]
        [Range(0.084, 1000.0)]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        [Display(Name = "Number of years")]
        public double? NumberOfYears { get; set; }*/

        [DataType(DataType.Currency)]
        [Display(Name = "Loan Amount")]
        [RegularExpression(@"^[.0-9]+$", ErrorMessage = "Invalid format")]
        public decimal LoanAmount { get; set; }

        [RegularExpression(@"^[0-9+]+$", ErrorMessage = "Please enter a valid Account Number")]
        [Display(Name = "Linked Account Number")]
        public int? LinkedAccountID { get; set; }
        public virtual ConsumerAccount LinkedAccount { get; set; }


    }


}

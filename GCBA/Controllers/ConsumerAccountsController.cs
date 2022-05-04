using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GCBA.Data;
using GCBA.Logic;
using GCBA.Models;

namespace GCBA.Controllers
{
    public class ConsumerAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ConsumerAccountLogic _consumerAccountLogic = new ConsumerAccountLogic();
        ConsumerDataAccess consumerData = new ConsumerDataAccess();
        DebitAndCreditLogic debitAndCredit = new DebitAndCreditLogic();
        // GET: ConsumerAccounts
        public async Task<ActionResult> Index()
        {
            var consumerAccounts = db.ConsumerAccounts.Include(c => c.Branch).Include(c => c.Consumer).Include(c => c.LinkedAccount);
            return View(await consumerAccounts.ToListAsync());
        }

        // GET: ConsumerAccounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerAccount consumerAccount = await db.ConsumerAccounts.FindAsync(id);
            if (consumerAccount == null)
            {
                return HttpNotFound();
            }
            return View(consumerAccount);
        }

        // GET: ConsumerAccounts/Create
        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name");
            ViewBag.ConsumerID = new SelectList(db.Consumers, "ID", "ConsumerInfo");
            ViewBag.LinkedAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountName");
            return View();
        }

        // POST: ConsumerAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AccountName,AccountNumber,AccountBalance,BranchID,AccountStatus,AccountType,ConsumerID,LoanMonthlyInterestRepay,LoanMonthlyRepay,LoanMonthlyPrincipalRepay,LoanPrincipalRemaining,TermsOfLoan,LoanAmount,LinkedAccountID")] ConsumerAccount consumerAccount)
        {
            if (ModelState.IsValid)
            {
                if (consumerAccount.AccountType == AccountType.Savings ||
                    consumerAccount.AccountType == AccountType.Current)
                {
                    consumerAccount.AccountBalance = 0;
                    consumerAccount.AccountNumber =
                        //_customerAccountLogic.CreateAccountNumber(customerAccount, customerAccount.AccountType);
                        _consumerAccountLogic.CreateAccountNumber(consumerAccount.AccountType, consumerAccount);

                    db.ConsumerAccounts.Add(consumerAccount);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("CreateLoan");

            }

            var savcurList = new List<AccountType> {AccountType.Savings, AccountType.Current};
            
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", consumerAccount.BranchID);
            ViewBag.ConsumerID = new SelectList(db.Consumers, "ID", "ConsumerInfo", consumerAccount.ConsumerID);
            ViewBag.LinkedAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountName", consumerAccount.LinkedAccountID);
            return RedirectToAction("CreateLoan");
        }

        // GET: ConsumerAccounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerAccount consumerAccount = await db.ConsumerAccounts.FindAsync(id);
            if (consumerAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", consumerAccount.BranchID);
            ViewBag.ConsumerID = new SelectList(db.Consumers, "ID", "ConsumerInfo", consumerAccount.ConsumerID);
            ViewBag.LinkedAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountName", consumerAccount.LinkedAccountID);
            return View(consumerAccount);
        }

        // POST: ConsumerAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AccountName,AccountNumber,AccountBalance,BranchID,AccountStatus,AccountType,ConsumerID,LoanMonthlyInterestRepay,LoanMonthlyRepay,LoanMonthlyPrincipalRepay,LoanPrincipalRemaining,TermsOfLoan,LoanAmount,LinkedAccountID,LoanInterestRatePerMonth,DaysCount,dailyInterestAccrued,SavingsWithdrawalCount,CurrentLien")] ConsumerAccount consumerAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consumerAccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", consumerAccount.BranchID);
            ViewBag.ConsumerID = new SelectList(db.Consumers, "ID", "ConsumerInfo", consumerAccount.ConsumerID);
            ViewBag.LinkedAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountName", consumerAccount.LinkedAccountID);
            return View(consumerAccount);
        }

        // GET: ConsumerAccounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerAccount consumerAccount = await db.ConsumerAccounts.FindAsync(id);
            if (consumerAccount == null)
            {
                return HttpNotFound();
            }
            return View(consumerAccount);
        }

        // POST: ConsumerAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ConsumerAccount consumerAccount = await db.ConsumerAccounts.FindAsync(id);
            db.ConsumerAccounts.Remove(consumerAccount);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }







        public ActionResult CreateLoan()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name");
            ViewBag.ConsumerID = new SelectList(db.Consumers, "ID", "ConsumerInfo");
            ViewBag.LinkedAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountNumber");
            return View();
        }

        // POST: ConsumerAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLoan([Bind(Include = "ID,AccountName,AccountNumber,AccountBalance,BranchID,AccountStatus,AccountType,ConsumerID,LoanMonthlyInterestRepay,LoanMonthlyRepay,LoanMonthlyPrincipalRepay,LoanPrincipalRemaining,TermsOfLoan,LoanAmount,LinkedAccountID")] ConsumerAccount consumerAccount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    consumerAccount.AccountBalance = 0;
                    consumerAccount.AccountType = AccountType.Loan;
                    consumerAccount.AccountNumber =
                        //_customerAccountLogic.CreateAccountNumber(customerAccount, customerAccount.AccountType);
                        _consumerAccountLogic.CreateAccountNumber(AccountType.Loan, consumerAccount);


                    var linkedID = consumerAccount.LinkedAccountID.GetValueOrDefault();
                    ConsumerAccount linkedConsumerAccount = null;
                    if (linkedID != 0)
                    {
                        linkedConsumerAccount =
                            db.ConsumerAccounts.Where(c => c.ID == linkedID).SingleOrDefault();
                    }

                    if (linkedConsumerAccount == null)
                    {
                        ReturnView("servicing account number does not exist", consumerAccount);
                        return View(consumerAccount);
                    }
                    // check if servicing account number actually belongs to customer and is either savings or current.
                    if (linkedConsumerAccount.AccountType == AccountType.Loan || linkedConsumerAccount.ConsumerID != consumerAccount.ConsumerID)
                    {
                        ReturnView("Invalid Linked Account", consumerAccount);
                        return View(consumerAccount);
                    }

                    if (linkedConsumerAccount.AccountStatus == AccountStatus.Closed)
                    {
                        ReturnView("Linked Account is Closed", consumerAccount);
                        return View(consumerAccount);
                    }

                    linkedID = linkedID;

                    consumerAccount.LoanInterestRatePerMonth = Convert.ToDecimal(2);

                    switch (consumerAccount.TermsOfLoan)
                    {
                        case TermsOfLoan.Fixed:
                            _consumerAccountLogic.ComputeFixedRepayment(consumerAccount, 1, 2);
                            break;
                        case TermsOfLoan.Reducing:
                            _consumerAccountLogic.ComputeReducingRepayment(consumerAccount, 1, 2);
                            break;
                        default:
                            break;
                    }
                    // loan disbursement
                    debitAndCredit.DebitConsumerAccount(consumerAccount, consumerAccount.LoanAmount);
                    debitAndCredit.CreditConsumerAccount(linkedConsumerAccount, consumerAccount.LoanAmount);














                    db.ConsumerAccounts.Add(consumerAccount);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ReturnView(ex.ToString(), consumerAccount);
                    return View(consumerAccount);
                }

            }

            ReturnView("enter valid data", consumerAccount);
            return View(consumerAccount);
        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void AddError(string error)
        {
            ModelState.AddModelError("", error);
        }

        private void ReturnView(string error, ConsumerAccount consumerAccount)
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", consumerAccount.BranchID);
            ViewBag.ConsumerID = new SelectList(db.Consumers, "ID", "ConsumerInfo", consumerAccount.ConsumerID);
            ViewBag.LinkedAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountNumber", consumerAccount.LinkedAccountID);
            AddError(error);
            
        }
    }
}

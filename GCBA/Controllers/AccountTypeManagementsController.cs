using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GCBA.Models;
using GCBA.Repositories;

namespace GCBA.Controllers
{
    public class AccountTypeManagementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        GlAccountDataAccess glData = new GlAccountDataAccess();

        // GET: AccountTypeManagements
        public async Task<ActionResult> Index()
        {
            var accountTypeManagements = db.AccountTypeManagements.Include(a => a.COTIncomeGl).Include(a => a.CurrentInterestExpenseGl).Include(a => a.LoanInterestIncomeGl).Include(a => a.LoanInterestReceivableGl).Include(a => a.SavingsInterestExpenseGl).Include(a => a.SavingsInterestPayableGl);
            return View(await accountTypeManagements.ToListAsync());
        }


        public ActionResult Info()
        {
            AccountTypeManagement accountConfiguration = db.AccountTypeManagements.First();
            //AccountConfiguration accountConfiguration = db.AccountConfigurations.Include(a => a.CurrentCotIncomeGl).Include(a => a.CurrentInterestExpenseGl).Include(a => a.LoanInterestExpenseGl).Include(a => a.LoanInterestIncomeGl).Include(a => a.LoanInterestReceivableGl).Include(a => a.SavingsInterestExpenseGl).Include(a => a.SavingsInterestPayableGl).First();
            if (accountConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(accountConfiguration);
        }

        // GET: AccountTypeManagements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTypeManagement accountTypeManagement = await db.AccountTypeManagements.FindAsync(id);
            if (accountTypeManagement == null)
            {
                return HttpNotFound();
            }
            return View(accountTypeManagement);
        }

        // GET: AccountTypeManagements/Create
        public ActionResult Create()
        {
            ViewBag.COTIncomeGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.CurrentInterestExpenseGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.LoanInterestIncomeGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.LoanInterestReceivableGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.SavingsInterestExpenseGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.SavingsInterestPayableGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            return View();
        }

        // POST: AccountTypeManagements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CurrentCreditInterestRate,CurrentMinimumBalance,COT,CurrentInterestExpenseGlID,COTIncomeGlID,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestReceivableGlID,IsOpened,FinancialDate")] AccountTypeManagement accountTypeManagement)
        {
            if (ModelState.IsValid)
            {
                accountTypeManagement.FinancialDate = DateTime.Now; 
                db.AccountTypeManagements.Add(accountTypeManagement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.COTIncomeGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountTypeManagement.COTIncomeGlID);
            ViewBag.CurrentInterestExpenseGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountTypeManagement.CurrentInterestExpenseGlID);
            ViewBag.LoanInterestIncomeGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountTypeManagement.LoanInterestIncomeGlID);
            ViewBag.LoanInterestReceivableGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountTypeManagement.LoanInterestReceivableGlID);
            ViewBag.SavingsInterestExpenseGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountTypeManagement.SavingsInterestExpenseGlID);
            ViewBag.SavingsInterestPayableGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountTypeManagement.SavingsInterestPayableGlID);
            return View(accountTypeManagement);
        }

        // GET: AccountTypeManagements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTypeManagement accountTypeManagement = await db.AccountTypeManagements.FindAsync(id);
            if (accountTypeManagement == null)
            {
                return HttpNotFound();
            }
            InitializeGetViewBags(accountTypeManagement);
            return View(accountTypeManagement);
        }

        // POST: AccountTypeManagements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CurrentCreditInterestRate,CurrentMinimumBalance,COT,CurrentInterestExpenseGlID,COTIncomeGlID,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestReceivableGlID,IsOpened,FinancialDate")] AccountTypeManagement accountTypeManagement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountTypeManagement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            InitializePostViewBags(accountTypeManagement);
            return View(accountTypeManagement);
        }




        // GET: AccountTypeManagements/Edit/5
        public async Task<ActionResult> EditSavings()
        {
            AccountTypeManagement accountTypeManagement = db.AccountTypeManagements.First();
            if (accountTypeManagement == null)
            {
                return HttpNotFound();
            }
            InitializeGetViewBags(accountTypeManagement);
            return View(accountTypeManagement);
        }

        // POST: AccountTypeManagements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSavings([Bind(Include = "ID,CurrentCreditInterestRate,CurrentMinimumBalance,COT,CurrentInterestExpenseGlID,COTIncomeGlID,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestReceivableGlID,IsOpened,FinancialDate")] AccountTypeManagement accountTypeManagement)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(accountTypeManagement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            InitializePostViewBags(accountTypeManagement);
            return View(accountTypeManagement);
        }

        public async Task<ActionResult> EditCurrent()
        {
            AccountTypeManagement accountTypeManagement = db.AccountTypeManagements.First();
            if (accountTypeManagement == null)
            {
                return HttpNotFound();
            }
            InitializeGetViewBags(accountTypeManagement);
            return View(accountTypeManagement);
        }

        // POST: AccountTypeManagements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCurrent([Bind(Include = "ID,CurrentCreditInterestRate,CurrentMinimumBalance,COT,CurrentInterestExpenseGlID,COTIncomeGlID,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestReceivableGlID,IsOpened,FinancialDate")] AccountTypeManagement accountTypeManagement)
        {
            if (ModelState.IsValid)
            {

                db.Entry(accountTypeManagement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            InitializePostViewBags(accountTypeManagement);
            return View(accountTypeManagement);
        }



        public async Task<ActionResult> EditLoan()
        {
            AccountTypeManagement accountTypeManagement = db.AccountTypeManagements.First();
            if (accountTypeManagement == null)
            {
                return HttpNotFound();
            }
            InitializeGetViewBags(accountTypeManagement);
            return View(accountTypeManagement);
        }

        // POST: AccountTypeManagements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditLoan([Bind(Include = "ID,CurrentCreditInterestRate,CurrentMinimumBalance,COT,CurrentInterestExpenseGlID,COTIncomeGlID,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestReceivableGlID,IsOpened,FinancialDate")] AccountTypeManagement accountTypeManagement)
        {
            if (ModelState.IsValid)
            {

                db.Entry(accountTypeManagement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            InitializePostViewBags(accountTypeManagement);
            return View(accountTypeManagement);
        }

















        // GET: AccountTypeManagements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTypeManagement accountTypeManagement = await db.AccountTypeManagements.FindAsync(id);
            if (accountTypeManagement == null)
            {
                return HttpNotFound();
            }
            return View(accountTypeManagement);
        }

        // POST: AccountTypeManagements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AccountTypeManagement accountTypeManagement = await db.AccountTypeManagements.FindAsync(id);
            db.AccountTypeManagements.Remove(accountTypeManagement);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void InstantiateGlAccounts(AccountTypeManagement accountTypeManagement)
        {
            if (accountTypeManagement.SavingsInterestExpenseGlID == 0) accountTypeManagement.SavingsInterestExpenseGlID = null;
            if (accountTypeManagement.SavingsInterestPayableGlID == 0) accountTypeManagement.SavingsInterestPayableGlID = null;
            if (accountTypeManagement.COTIncomeGlID == 0) accountTypeManagement.COTIncomeGlID = null;
            if (accountTypeManagement.CurrentInterestExpenseGlID == 0) accountTypeManagement.CurrentInterestExpenseGlID = null;
            if (accountTypeManagement.LoanInterestIncomeGlID == 0) accountTypeManagement.LoanInterestIncomeGlID = null;
            if (accountTypeManagement.LoanInterestReceivableGlID == 0) accountTypeManagement.LoanInterestReceivableGlID = null;
        }

        public void InitializeGetViewBags(AccountTypeManagement accountTypeManagement)
        {
            var allAssets = glData.GetAllAssetAccounts();
            var allLiabilities = glData.GetAllLiabilityAccounts();
            var allIncomes = glData.GetAllIncomeAccounts();
            var allExpenses = glData.GetAllExpenseAccounts();



            ViewBag.COTIncomeGlID = new SelectList(allIncomes, "ID", "AccountName", accountTypeManagement.COTIncomeGlID);
            ViewBag.CurrentInterestExpenseGlID = new SelectList(allExpenses, "ID", "AccountName", accountTypeManagement.CurrentInterestExpenseGlID);
            ViewBag.LoanInterestIncomeGlID = new SelectList(allIncomes, "ID", "AccountName", accountTypeManagement.LoanInterestIncomeGlID);
            ViewBag.LoanInterestReceivableGlID = new SelectList(allAssets, "ID", "AccountName", accountTypeManagement.LoanInterestReceivableGlID);
            ViewBag.SavingsInterestExpenseGlID = new SelectList(allExpenses, "ID", "AccountName", accountTypeManagement.SavingsInterestExpenseGlID);
            ViewBag.SavingsInterestPayableGlID = new SelectList(allLiabilities, "ID", "AccountName", accountTypeManagement.SavingsInterestPayableGlID);
        }

        public void InitializePostViewBags(AccountTypeManagement accountTypeManagement)
        {

            var allAssets = glData.GetAllAssetAccounts();
            var allLiabilities = glData.GetAllLiabilityAccounts();
            var allIncomes = glData.GetAllIncomeAccounts();
            var allExpenses = glData.GetAllExpenseAccounts();


            ViewBag.COTIncomeGlID = new SelectList(allIncomes, "ID", "AccountName", accountTypeManagement.COTIncomeGlID);
            ViewBag.CurrentInterestExpenseGlID = new SelectList(allExpenses, "ID", "AccountName", accountTypeManagement.CurrentInterestExpenseGlID);
            ViewBag.LoanInterestIncomeGlID = new SelectList(allIncomes, "ID", "AccountName", accountTypeManagement.LoanInterestIncomeGlID);
            ViewBag.LoanInterestReceivableGlID = new SelectList(allAssets, "ID", "AccountName", accountTypeManagement.LoanInterestReceivableGlID);
            ViewBag.SavingsInterestExpenseGlID = new SelectList(allExpenses, "ID", "AccountName", accountTypeManagement.SavingsInterestExpenseGlID);
            ViewBag.SavingsInterestPayableGlID = new SelectList(allLiabilities, "ID", "AccountName", accountTypeManagement.SavingsInterestPayableGlID);
        }
    }
}

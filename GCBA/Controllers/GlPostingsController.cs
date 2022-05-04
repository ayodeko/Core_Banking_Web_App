using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GCBA.Logic;
using GCBA.Models;
using Microsoft.AspNet.Identity;

namespace GCBA.Controllers
{
    public class GlPostingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        EodLogic eodLogic = new EodLogic();
        DebitAndCreditLogic debitAndCredit =  new DebitAndCreditLogic();
        private FinancialReportLogic financialLogic = new FinancialReportLogic();

        public string GetLoggedInUserId()
        {
            return User.Identity.GetUserId();
        }

        // GET: GlPostings
        public async Task<ActionResult> Index()
        {
            var glPostings = db.GlPostings.Include(g => g.CrGlAccount).Include(g => g.DrGlAccount);
            return View(await glPostings.ToListAsync());
        }

        public ActionResult UserPosts()
        {
            string userId = GetLoggedInUserId();
            var userGlPostings = db.GlPostings.Include(g => g.CrGlAccount).Include(g => g.DrGlAccount).Where(g => g.PostInitiatorId.Equals(userId));
            //return View("Index", userGlPostings.ToList());
            return View(userGlPostings.ToList());
        }

        // GET: GlPostings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlPosting glPosting = await db.GlPostings.FindAsync(id);
            if (glPosting == null)
            {
                return HttpNotFound();
            }
            return View(glPosting);
        }

        // GET: GlPostings/Create
        public ActionResult Create(int? crId, int? drId)
        {
            if (crId == null || drId == null || crId == drId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount drglAccount = db.GlAccounts.Find(drId);
            GlAccount crglAccount = db.GlAccounts.Find(crId);
            if (drglAccount == null || crglAccount == null)
            {
                return HttpNotFound();
            }

            GlPosting model = new GlPosting();
            model.DrGlAccountID = drglAccount.ID;
            model.CrGlAccountID = crglAccount.ID;

            return View(model);
        }

        // POST: GlPostings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,CreditAmount,DebitAmount,Narration,Date,DrGlAccountID,CrGlAccountID,PostInitiatorId,Status")] GlPosting glPosting)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var drAct = db.GlAccounts.Find(glPosting.DrGlAccountID);
                    var crAct = db.GlAccounts.Find(glPosting.CrGlAccountID);

                    if (crAct != null)
                    {
                        if (crAct.AccountName.ToLower().Contains("till") ||
                            crAct.AccountName.ToLower().Contains("vault"))
                        {
                            if (crAct.AccountBalance < glPosting.CreditAmount)
                            {

                                AddError("Insufficient funds in asset account to be credited");
                                return View(glPosting);
                            }
                        }
                    }
                    else { AddError("credit account is read as null"); return View(glPosting);}

                    glPosting.Status = PostStatus.Pending;

                    decimal amt = glPosting.CreditAmount;

                    if (crAct.GlCategory.MainGlCategory == MainGlCategory.Asset || crAct.GlCategory.MainGlCategory == MainGlCategory.Expense)
                    {
                        if (crAct.AccountBalance < amt)
                        {
                            AddError("Insufficient funds in the asset or expense account to be credited");
                            return View(glPosting);
                        }
                    }

                    if (drAct.GlCategory.MainGlCategory == MainGlCategory.Capital ||
                        drAct.GlCategory.MainGlCategory == MainGlCategory.Liability ||
                        drAct.GlCategory.MainGlCategory == MainGlCategory.Income)
                    {
                        if (drAct.AccountBalance < amt)
                        {
                            AddError("Insufficient funds in the account to be debited");
                            return View(glPosting);
                        }
                    }
                    debitAndCredit.CreditGl(crAct, amt);
                    debitAndCredit.DebitGl(drAct, amt);

                    glPosting.Status = PostStatus.Approved;

                    glPosting.PostInitiatorId = User.Identity.Name;
                    glPosting.Date = DateTime.Now;

                    financialLogic.CreateTransaction(drAct, glPosting.DebitAmount, TransactionType.Debit);
                    financialLogic.CreateTransaction(crAct, glPosting.CreditAmount, TransactionType.Credit);

                    db.GlPostings.Add(glPosting);
                    db.SaveChanges();
                    return RedirectToAction("GlPostSuccess");
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());
                    return View(glPosting);
                }
            }


            AddError("Please, enter valid data");
            return View(glPosting);
        }

        // GET: GlPostings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlPosting glPosting = await db.GlPostings.FindAsync(id);
            if (glPosting == null)
            {
                return HttpNotFound();
            }
            ViewBag.CrGlAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", glPosting.CrGlAccountID);
            ViewBag.DrGlAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", glPosting.DrGlAccountID);
            return View(glPosting);
        }

        // POST: GlPostings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,CreditAmount,DebitAmount,Narration,Date,DrGlAccountID,CrGlAccountID,PostInitiatorId,Status")] GlPosting glPosting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(glPosting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CrGlAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", glPosting.CrGlAccountID);
            ViewBag.DrGlAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", glPosting.DrGlAccountID);
            return View(glPosting);
        }

        // GET: GlPostings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlPosting glPosting = await db.GlPostings.FindAsync(id);
            if (glPosting == null)
            {
                return HttpNotFound();
            }
            return View(glPosting);
        }

        // POST: GlPostings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GlPosting glPosting = await db.GlPostings.FindAsync(id);
            db.GlPostings.Remove(glPosting);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        public ActionResult SelectFirstAccount()
        {
           /* if (eodLogic.isBusinessClosed())
            {
                
                return View(new List<GlAccount>());
            }*/
            return View(db.GlAccounts.ToList());
        }

        public ActionResult SelectSecondAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount crglAccount = db.GlAccounts.Find(id);
            if (crglAccount == null)
            {
                return HttpNotFound();
            }


            ViewBag.CrGlAccountID = crglAccount.ID;
            var acts = db.GlAccounts.Where(g => g.ID != crglAccount.ID && g.GlCategory.MainGlCategory == crglAccount.GlCategory.MainGlCategory).ToList();
            return View(acts);
        }


        public ActionResult GlPostSuccess()
        {
            return View();
        }

        public ActionResult GlPostFail()
        {
            return View();
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
    }
}

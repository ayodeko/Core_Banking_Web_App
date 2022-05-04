using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using GCBA.Logic;
using GCBA.Models;
using Microsoft.AspNet.Identity;

namespace GCBA.Controllers
{
    public class TellerPostingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ConsumerAccountLogic consumerAccountLogic = new ConsumerAccountLogic();
        private TellerPostingLogic tellerPostingLogic = new TellerPostingLogic();
        FinancialReportLogic financialLogic = new FinancialReportLogic();

        // GET: TellerPostings
        public async Task<ActionResult> Index()
        {
            var tellerPostings = db.TellerPostings.Include(t => t.CustomerAccount).Include(t => t.TillAccount);
            return View(await tellerPostings.ToListAsync());
        }

        private string GetLoggedInUserId()
        {
            var currentUserId = User.Identity.GetUserId();
            return currentUserId;
        }

        // GET: TellerPostings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TellerPosting tellerPosting = await db.TellerPostings.FindAsync(id);
            if (tellerPosting == null)
            {
                return HttpNotFound();
            }
            return View(tellerPosting);
        }

        // GET: TellerPostings/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumerAccount customerAccount = db.ConsumerAccounts.Find(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }

            TellerPosting model = new TellerPosting();
            model.ConsumerAccountID = customerAccount.ID;

            ViewBag.PostingType = string.Empty;

            return View(model);
            
        }

        // POST: TellerPostings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Amount,Narration,Date,PostingType,ConsumerAccountID,PostInitiatorId,TillAccountID,Status")] TellerPosting tellerPosting)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    // Get present user
                    string tellerId = GetLoggedInUserId();

                    //check if the user has a till account to know if it is a teller

                    bool tellerHasTill = db.TillAccounts.Any(tu => tu.UserId.Equals(tellerId));
                    if (!tellerHasTill)
                    {
                        AddError("No till associated with logged in teller");
                        return View(tellerPosting);
                    }
                    //Get the till account ID of the current user
                    int tillId = db.TillAccounts.Where(tu => tu.UserId.Equals(tellerId)).First().GlAccountID;
                    

                    tellerPosting.TillAccountID = tillId;
                    //Get the till account of the current user
                    var tillAct = db.GlAccounts.Find(tillId);


                    var custAct = db.ConsumerAccounts.Find(tellerPosting.ConsumerAccountID);

                    tellerPosting.PostInitiatorId = tellerId;
                    tellerPosting.Date = DateTime.Now;

                    var amount = tellerPosting.Amount;
                    if (tellerPosting.PostingType == TellerPostingType.Withdrawal)
                    {
                        if (consumerAccountLogic.CheckIfAccountBalanceIsEnough(custAct, tellerPosting.Amount))
                        {
                            if (tillAct.AccountBalance <= tellerPosting.Amount)
                            {
                                AddError("Insuficient funds in till account");
                                return View(tellerPosting);
                            }
                        }
                        else
                        {
                            AddError("Insuficient funds in Customer account");
                            return View(tellerPosting);
                        }



                        tellerPosting.Status = PostStatus.Pending;
                        string result = tellerPostingLogic.PostTeller(custAct, tillAct, amount, TellerPostingType.Withdrawal);
                        if (!result.Equals("success"))
                        {
                            return RedirectToAction("TellerPostFail", new { message = result });
                        }

                        tellerPosting.Status = PostStatus.Approved;
                        db.TellerPostings.Add(tellerPosting);
                        await db.SaveChangesAsync();
                        return RedirectToAction("TellerPostSuccess");
                    }
                    else
                    {
                        //Teller Posting For Deposit
                        tellerPosting.Status = PostStatus.Pending;

                        {
                            //The teller is changed to Successful

                            string result =
                                tellerPostingLogic.PostTeller(custAct, tillAct, amount, TellerPostingType.Deposit);
                            if (!result.Equals("success"))
                            {
                                return RedirectToAction("TellerPostFail", new {message = result});
                            }

                            tellerPosting.Status = PostStatus.Approved;

                            //record transaction
                            financialLogic.CreateTransaction(tillAct, amount, TransactionType.Debit);
                            financialLogic.CreateTransaction(custAct, amount, TransactionType.Credit);


                        }
                        //db.SaveChanges();

                        db.TellerPostings.Add(tellerPosting);
                        db.SaveChanges();
                        return RedirectToAction("TellerPostSuccess");
                    }
                }
                catch 
                {

                }
            }

            ViewBag.ConsumerAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountName", tellerPosting.ConsumerAccountID);
            ViewBag.TillAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", tellerPosting.TillAccountID);
            return View(tellerPosting);
        }

        // GET: TellerPostings/Edit/5
        /*public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TellerPosting tellerPosting = await db.TellerPostings.FindAsync(id);
            if (tellerPosting == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConsumerAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountName", tellerPosting.ConsumerAccountID);
            ViewBag.TillAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", tellerPosting.TillAccountID);
            return View(tellerPosting);
        }

        // POST: TellerPostings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Amount,Narration,Date,PostingType,ConsumerAccountID,PostInitiatorId,TillAccountID,Status")] TellerPosting tellerPosting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tellerPosting).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ConsumerAccountID = new SelectList(db.ConsumerAccounts, "ID", "AccountName", tellerPosting.ConsumerAccountID);
            ViewBag.TillAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", tellerPosting.TillAccountID);
            return View(tellerPosting);
        }*/

        // GET: TellerPostings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TellerPosting tellerPosting = await db.TellerPostings.FindAsync(id);
            if (tellerPosting == null)
            {
                return HttpNotFound();
            }
            return View(tellerPosting);
        }

        // POST: TellerPostings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TellerPosting tellerPosting = await db.TellerPostings.FindAsync(id);
            db.TellerPostings.Remove(tellerPosting);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public ActionResult SelectAccount()
        {
            // check whether user has till here and bounce? or later in create's post ?

            return View(db.ConsumerAccounts.Include(a => a.Branch).Where(a => a.AccountType != AccountType.Loan).ToList());
        }



        public ActionResult UserPosts()
        {
            string userId = GetLoggedInUserId();
            var userTellerPostings = db.TellerPostings.Include(t => t.CustomerAccount).Where(t => t.PostInitiatorId.Equals(userId));
            //return View("Index", userTellerPostings.ToList());
            return View(userTellerPostings.ToList());
        }


        public ActionResult TellerPostFail()
        {
            return View();
        }

        public ActionResult TellerPostSuccess()
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

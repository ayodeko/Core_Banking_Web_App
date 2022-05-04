using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GCBA.Models;
using GCBA.Models.ViewModels;
using GCBA.Repositories;

namespace GCBA.Controllers
{
    public class TellersController : Controller
    {



        private ApplicationDbContext db = new ApplicationDbContext();
        TellerDataAccess tellerData = new TellerDataAccess();
        private GlAccountDataAccess glData = new GlAccountDataAccess();

        // GET: TillAccounts
        public async Task<ActionResult> Index()
        {
            var tellerDetails = tellerData.GetAllTellerDetails();
            var viewModel = new List<TillAccountViewModel>();

            foreach (var detail in tellerDetails)
            {
                TillAccountViewModel data;
                if (detail.GlAccountID == 0)
                {
                    data = new TillAccountViewModel
                    {
                        GLAccountName = "--", AccountBalance = "--", Username = db.Users.Find(detail.UserId).UserName,
                        HasDetails = false, IsDeletable = false
                    };
                    viewModel.Add(data);
                }
                else
                {
                    var applicationUser = db.Users.Find(detail.UserId);
                    data = new TillAccountViewModel
                    {
                        GLAccountName = detail.GlAccount.AccountName, Id = detail.Id,
                        Username = applicationUser.UserName, AccountBalance = detail.GlAccount.AccountBalance.ToString(CultureInfo.InvariantCulture)
                    };
                    viewModel.Add(data);
                }
            }


            //var tillAccounts = db.TillAccounts.Include(t => t.GlAccount);
            return View(viewModel);
        }

        // GET: TillAccounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TillAccount tillAccount = await db.TillAccounts.FindAsync(id);
            if (tillAccount == null)
            {
                return HttpNotFound();
            }
            return View(tillAccount);
        }

        // GET: TillAccounts/Create
        public ActionResult Create()
        {
            var testList = new List<string> {"a", "b", "c"};

            ViewBag.Users = new SelectList(tellerData.GetTellersWithNoTills(), "Id", "UserName");
            ViewBag.GlAccountID = new SelectList(glData.GetTillsWithoutTellers(), "ID", "AccountName");
            return View();
        }

        // POST: TillAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserId,GlAccountID")] TillAccount tillAccount)
        {
            if (ModelState.IsValid)
            {
                db.TillAccounts.Add(tillAccount);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Users = new SelectList(tellerData.GetTellersWithNoTills(), "Id", "UserName", tillAccount.UserId);
            ViewBag.GlAccountID = new SelectList(tellerData.GetTellersWithNoTills(), "ID", "AccountName", tillAccount.GlAccountID);
            return View(tillAccount);
        }

        // GET: TillAccounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TillAccount tillAccount = await db.TillAccounts.FindAsync(id);
            if (tillAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.GlAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", tillAccount.GlAccountID);
            return View(tillAccount);
        }

        // POST: TillAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,GlAccountID")] TillAccount tillAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tillAccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.GlAccountID = new SelectList(db.GlAccounts, "ID", "AccountName", tillAccount.GlAccountID);
            return View(tillAccount);
        }

        // GET: TillAccounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TillAccount tillAccount = await db.TillAccounts.FindAsync(id);
            if (tillAccount == null)
            {
                return HttpNotFound();
            }
            return View(tillAccount);
        }

        // POST: TillAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TillAccount tillAccount = await db.TillAccounts.FindAsync(id);
            db.TillAccounts.Remove(tillAccount);
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
    }
}

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
//using GCBA.Migrations;
using GCBA.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GCBA.Controllers
{
    public class GlAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        GlAccountLogic glAccountLogic = new GlAccountLogic();
        // GET: GlAccounts
        public async Task<ActionResult> Index()
        {
            var glAccounts = db.GlAccounts.Include(g => g.Branch).Include(g => g.GlCategory);
            return View(await glAccounts.ToListAsync());
        }

        // GET: GlAccounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount glAccount = await db.GlAccounts.FindAsync(id);
            if (glAccount == null)
            {
                return HttpNotFound();
            }
            return View(glAccount);
        }

        // GET: GlAccounts/Create
        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name");
            ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name");
            return View();
        }

        // POST: GlAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AccountName,Code,AccountBalance,BranchID,GlCategoryID")] GlAccount glAccount)
        {
            if (ModelState.IsValid)
            {
                ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", glAccount.BranchID);
                ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name", glAccount.GlCategoryID);

                if (!glAccountLogic.IsUnique(glAccount.AccountName))
                {
                    AddError("GL Account Name already exists");
                    return View("Create");
                }

                GlCategory glCategory = db.GlCategories.Find(glAccount.GlCategoryID);

                glAccount.Code = glAccountLogic.GenerateGLAccountNumber(glCategory.MainGlCategory);
                glAccount.AccountBalance = 0;
                db.GlAccounts.Add(glAccount);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(glAccount);
        }

        // GET: GlAccounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount glAccount = await db.GlAccounts.FindAsync(id);
            if (glAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", glAccount.BranchID);
            ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name", glAccount.GlCategoryID);
            return View(glAccount);
        }

        // POST: GlAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AccountName,BranchID,GlCategoryID")]
            GlAccount glAccount)
        {
            if (ModelState.IsValid)
            {
                GlAccount dbGlAccount = await db.GlAccounts.FindAsync(glAccount.ID);

                ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", glAccount.BranchID);
                ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name", glAccount.GlCategoryID);
                try
                {
                    GlAccount originalAccount = db.GlAccounts.Find(glAccount.ID);
                    db.Entry(originalAccount).State = EntityState.Detached;

                    string originalName = originalAccount.AccountName;
                    if (!glAccount.AccountName.ToLower().Equals(originalName.ToLower()))
                    {
                        if (!glAccountLogic.IsUnique(glAccount.AccountName))
                        {
                            AddError("Please select another name");
                            return View(glAccount);
                        }
                    }
                    glAccount.Code = originalAccount.Code;
                    glAccount.AccountBalance = originalAccount.AccountBalance;
                    glAccount.GlCategoryID = originalAccount.GlCategoryID;

                    db.Entry(glAccount).State = EntityState.Modified;
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());
                    return View(glAccount);
                }

                /*glAccount.Code = dbGlAccount.Code;
                glAccount.AccountBalance = dbGlAccount.AccountBalance;
                glAccount.GlCategoryID = dbGlAccount.GlCategoryID;*/

            }
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", glAccount.BranchID);
            ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name", glAccount.GlCategoryID);
            AddError("Please enter valid data");
            return View(glAccount);
        }

        // GET: GlAccounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount glAccount = await db.GlAccounts.FindAsync(id);
            if (glAccount == null)
            {
                return HttpNotFound();
            }
            return View(glAccount);
        }

        // POST: GlAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GlAccount glAccount = await db.GlAccounts.FindAsync(id);


            db.GlAccounts.Remove(glAccount);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult GlAccountDeleteError()
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

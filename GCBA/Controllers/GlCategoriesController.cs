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
using GCBA.Repositories;

namespace GCBA.Controllers
{
    public class GlCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        GlCategoryLogic glCatLogic = new GlCategoryLogic();

        private UserDataAccess userData = new UserDataAccess();

        // GET: GlCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.GlCategories.ToListAsync());
        }

        // GET: GlCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlCategory glCategory = await db.GlCategories.FindAsync(id);
            if (glCategory == null)
            {
                return HttpNotFound();
            }
            return View(glCategory);
        }

        // GET: GlCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GlCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,MainGlCategory,Description,Code")] GlCategory glCategory)
        {

            if (ModelState.IsValid)
            {
                if (!glCatLogic.isUnique(glCategory.Name))
                {
                    AddError("The GL Category Name already exists");
                    return View(glCategory);
                }

                glCategory.Code = glCatLogic.CreateGlCategoryCode(glCategory);
                db.GlCategories.Add(glCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(glCategory);
        }

        // GET: GlCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlCategory glCategory = await db.GlCategories.FindAsync(id);
            if (glCategory == null)
            {
                return HttpNotFound();
            }
            return View(glCategory);
        }

        // POST: GlCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,MainGlCategory,Description")] GlCategory glCategory)
        {
            if (ModelState.IsValid)
            {
                if (!glCatLogic.isUnique(glCategory.Name))
                db.Entry(glCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(glCategory);
        }

        // GET: GlCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlCategory glCategory = await db.GlCategories.FindAsync(id);
            if (glCategory == null)
            {
                return HttpNotFound();
            }
            return View(glCategory);
        }

        // POST: GlCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GlCategory glCategory = await db.GlCategories.FindAsync(id);


            var allGlAccount = db.GlAccounts.ToList();

            foreach (var acct in allGlAccount)
            {
                if (acct.GlCategoryID == id)
                {
                    AddError("GL Categories Cannot be deleted because it is linked to a GL Account");
                    return View("CategoryDeleteError");
                }

            }
            


            var tellers = userData.GetAll().Where(c => c.Role.Name == "Teller");
            var gglAccountTill = db.GlAccounts.Where(c => c.AccountName.ToLower().Contains("till")).ToList();

            db.GlCategories.Remove(glCategory);
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
        private void AddError(string error)
        {
            ModelState.AddModelError("", error);
        }
    }
}

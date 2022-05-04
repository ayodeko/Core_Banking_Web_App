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

namespace GCBA.Controllers
{
    public class FineNamesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FineNames
        public async Task<ActionResult> Index()
        {
            return View(await db.FineNames.ToListAsync());
        }

        // GET: FineNames/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FineNames fineNames = await db.FineNames.FindAsync(id);
            if (fineNames == null)
            {
                return HttpNotFound();
            }
            return View(fineNames);
        }

        // GET: FineNames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FineNames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] FineNames fineNames)
        {
            if (ModelState.IsValid)
            {
                db.FineNames.Add(fineNames);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(fineNames);
        }

        // GET: FineNames/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FineNames fineNames = await db.FineNames.FindAsync(id);
            if (fineNames == null)
            {
                return HttpNotFound();
            }
            return View(fineNames);
        }

        // POST: FineNames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] FineNames fineNames)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fineNames).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fineNames);
        }

        // GET: FineNames/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FineNames fineNames = await db.FineNames.FindAsync(id);
            if (fineNames == null)
            {
                return HttpNotFound();
            }
            return View(fineNames);
        }

        // POST: FineNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FineNames fineNames = await db.FineNames.FindAsync(id);
            db.FineNames.Remove(fineNames);
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

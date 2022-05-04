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
using Microsoft.Ajax.Utilities;

namespace GCBA.Controllers
{
    public class ConsumersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Consumers
        public async Task<ActionResult> Index()
        {
            return View(await db.Consumers.ToListAsync());
        }

        // GET: Consumers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = await db.Consumers.FindAsync(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // GET: Consumers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consumers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ConsumerLongID,FullName,Address,Email,PhoneNumber,Gender")] Consumer consumer)
        {
            if (ModelState.IsValid)
            {

                var customerLogic = new ConsumerLogic();
                consumer.ConsumerLongID = customerLogic.GenerateCustomerLongId();

                if (consumer.ConsumerLongID != null && !consumer.FullName.IsNullOrWhiteSpace())
                {
                    var consumerInfo = (consumer.FullName + " " + "(" + consumer.ConsumerLongID.ToString() + ")");
                    consumer.ConsumerInfo = consumerInfo;
                }

                db.Consumers.Add(consumer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(consumer);
        }

        // GET: Consumers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = await db.Consumers.FindAsync(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // POST: Consumers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ConsumerLongID,FullName,Address,Email,PhoneNumber,Gender")] Consumer consumer)
        {
            if (ModelState.IsValid)
            {

                if (consumer.ConsumerLongID != null && !consumer.FullName.IsNullOrWhiteSpace())
                {
                    var consumerInfo = (consumer.FullName + " " + "(" + consumer.ConsumerLongID.ToString() + ")");
                    consumer.ConsumerInfo = consumerInfo;
                }
                db.Entry(consumer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(consumer);
        }

        // GET: Consumers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = await db.Consumers.FindAsync(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // POST: Consumers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Consumer consumer = await db.Consumers.FindAsync(id);
            db.Consumers.Remove(consumer);
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

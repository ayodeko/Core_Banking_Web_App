using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GCBA.Logic;
using GCBA.Models;
using Microsoft.AspNet.Identity;

namespace GCBA.Controllers
{
    public class EodController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        
        
        EodLogic logic = new EodLogic();

        public ActionResult Index()
        {

            var result = logic.RunEOD();
            ViewBag.Result = result;
            return View();

        }

        public ActionResult OpenOrCloseBusiness()
        {
            AccountTypeManagement config =
                db.AccountTypeManagements.First();
            try
            {
                if (config.IsOpened == false)
                {
                    config.IsOpened = true;

                    logic.OpenBusiness();
                    string result = logic.RunEOD();
                    return RedirectToAction("Index");
                }
                else
                {
                    config.IsOpened = false;

                    db.Entry(config).State = EntityState.Modified;
                    db.SaveChanges();
                    string result = logic.RunEOD();
                    return RedirectToAction("Index", new {message = result});
                }
            }
            catch
            {
                AddError("EOD Could not Run.");
                return RedirectToAction("Index", new {message = "Error"});
            }

            return RedirectToAction("Index");
        }

        public ApplicationUser GetCurrentUser()
        {
            var currUser = User.Identity.GetUserId();
            return db.Users.Find(currUser);
        }
        private void AddError(string error)
        {
            ModelState.AddModelError("", error);
        }
    }
}
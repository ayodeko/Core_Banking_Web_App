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
using GCBA.Models.ViewModels.UserViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GCBA.Controllers
{
    public class UserController : Controller
    {


         #region managers
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion
        UtilityLogic ut =new UtilityLogic();

        private UserLogic user = new UserLogic();




        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public async Task<ActionResult> Index()
        {
            var applicationUsers = UserManager.Users.Include(a => a.Branch).Include(a => a.Role);
            return View(await applicationUsers.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name");
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel userViewModel)
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name");
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name");
            if (ModelState.IsValid)
            {
                var userList = UserManager.Users;
                if (userList.Any(c => c.UserName == userViewModel.Username))
                {
                    AddError("The Username Already Exists!");
                    return View(userViewModel);
                }

                if (userList.Any(c => c.Email == userViewModel.Email))
                {
                    AddError("The Email Already Exists!");
                    
                    return View(userViewModel);
                }

                ApplicationUser applicationUser = new ApplicationUser
                {
                    FullName = userViewModel.FullName, UserName = userViewModel.Username, Email = userViewModel.Email, BranchID = userViewModel.BranchID,
                    RoleID = userViewModel.RoleID, PhoneNumber = userViewModel.PhoneNumber
                };


                string newPassword = ut.GetRandomPassword();
                var result = 
                await UserManager.CreateAsync(applicationUser, newPassword);

                
                try
                {
                    user.SendPasswordToUser(applicationUser.FullName, applicationUser.Email, applicationUser.UserName,
                        newPassword);
                }
                catch
                {
                    ViewBag.Message = "Exception caught in mail";
                }
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
                
            }
            AddError("Error");
            return View(userViewModel);
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
             if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            //ApplicationUser applicationUser = userdb.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            //EditUserViewModel model = new EditUserViewModel { Id = applicationUser.Id, FullName = applicationUser.FullName, Username = applicationUser.UserName, Email = applicationUser.Email, PhoneNumber = applicationUser.PhoneNumber};

            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", applicationUser.BranchID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", applicationUser.RoleID);
            return View(applicationUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FullName,RoleID,BranchID,Email,PhoneNumber,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(applicationUser).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());
                }
            }
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", applicationUser.BranchID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", applicationUser.RoleID);
            return View(applicationUser);
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
                await UserManager.DeleteAsync(applicationUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                AddError(ex.ToString());
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index");
        }

        public void AddError(string error)
        {
            ModelState.AddModelError("", error);
        }
    }
}

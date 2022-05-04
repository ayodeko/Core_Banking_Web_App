using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GCBA.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }

        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        public int BranchID { get; set; }
        public virtual Branch Branch { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<FineNames> FineNames { get; set; }

        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }

        public DbSet<ConsumerAccount> ConsumerAccounts { get; set; }

        public DbSet<GlCategory> GlCategories { get; set; }

        public DbSet<GlAccount> GlAccounts { get; set; }


        public DbSet<TellerTill> TellerTills { get; set; }


        public DbSet<TillAccount> TillAccounts { get; set; }

        public DbSet<AccountTypeManagement> AccountTypeManagements { get; set; }

        public DbSet<TellerPosting> TellerPostings { get; set; }

        public DbSet<GlPosting> GlPostings { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<ExpenseIncomeEntry> ExpenseIncomeEntries { get; set; }

        public DbSet<ClientAccount> ClientAccounts { get; set; }

        //public DbSet<AccountConfiguration> AccountConfigurations { get; set; }  

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<GCBA.Models.ApplicationUser> ApplicationUsers { get; set; }



        //public System.Data.Entity.DbSet<GCBA.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}
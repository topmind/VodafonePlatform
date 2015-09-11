using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VodafoneWeb.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //static ApplicationDbContext()
        //{
        //    System.Data.Entity.Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        //    //Database.SetInitializer(new MySqlInitializer());
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Dealer>().HasMany(i => i.Sales).WithRequired().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Dealer>().HasMany(i => i.Inventories).WithRequired().WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<VodafoneWeb.Models.Dealer> Dealers { get; set; }

        public System.Data.Entity.DbSet<VodafoneWeb.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<VodafoneWeb.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<VodafoneWeb.Models.Plan> Plans { get; set; }

        public System.Data.Entity.DbSet<VodafoneWeb.Models.SalesTransaction> SalesTransactions { get; set; }

        public System.Data.Entity.DbSet<VodafoneWeb.Models.Inventory> Inventories { get; set; }

        public System.Data.Entity.DbSet<VodafoneWeb.Models.InvetoryChangeHistory> InvetoryChangeHistories { get; set; }

        public System.Data.Entity.DbSet<VodafoneWeb.Models.ProductCategory> ProductCategories { get; set; }
    }
}

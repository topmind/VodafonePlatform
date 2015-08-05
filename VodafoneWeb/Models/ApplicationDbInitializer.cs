﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace VodafoneWeb.Models
{
    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    //public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>     
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            if (!context.Database.Exists())
            {
                // if database did not exist before - create it
                context.Database.Create();
            }
            else
            {
                // query to check if MigrationHistory table is present in the database 
                var migrationHistoryTableExists = ((IObjectContextAdapter)context).ObjectContext.ExecuteStoreQuery<int>(
                string.Format(
                  "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '__MigrationHistory'",
                  "aspnetusers"));

                // if MigrationHistory table is not there (which is the case first time we run) - create it
                if (migrationHistoryTableExists.FirstOrDefault() == 0)
                {
                    context.Database.Delete();
                    context.Database.Create();
                }
            }
        }
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            InitializeApplicationData(context);
            base.Seed(context);
        }

        public static void InitializeApplicationData(ApplicationDbContext context)
        {
            var dealers = new List<Dealer>
            {
                new Dealer {Name = "Swanston", DealerCode = "IDC141"},
                new Dealer {Name = "Deakin", DealerCode = "IDC390"}
            };
            dealers.ForEach(d => context.Dealers.Add(d));

            var products = new List<Product>
            {
                new Product {IMEI = "354380061969018",Name="IPHONE 6 PLUS GOLD 16GB"},
                new Product {IMEI = "356977061501200", Name = "IPHONE 6 SILVER 16GB"}
            };
            products.ForEach(p => context.Products.Add(p));
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            //const string name = "admin@example.com";
            const string name = "admin";
            const string password = "admin";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                //user = new ApplicationUser { UserName = name, Email = name };
                user = new ApplicationUser { UserName = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }
}

using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using VodafoneWeb.Models;

namespace VodafoneWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<VodafoneWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VodafoneWeb.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            #region InitializeIdentityForEF
            //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            //const string name = "admin@example.com";
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true
                RequireUniqueEmail = false
            };
            // Configure validation logic for passwords
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            // Configure user lockout defaults
            userManager.UserLockoutEnabledByDefault = false;
            userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            userManager.MaxFailedAccessAttemptsBeforeLockout = 5;


            var roleStore = new RoleStore<ApplicationRole>(context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);
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


            #endregion

            #region InitializeApplicationData
            var dealers = new List<Dealer>
            {
                new Dealer {DealerName = "Burwood", DealerCode = "IDC167"},
                new Dealer {DealerName = "Carlton", DealerCode = "IDC141"},
                new Dealer {DealerName = "Malvern", DealerCode = "IDC052"}
            };
            dealers.ForEach(d => context.Dealers.Add(d));

            var products = new List<Product>
            {
                new Product {Name="IPHONE 6 PLUS GOLD 16GB"},
                new Product {Name = "IPHONE 6 SILVER 16GB"}
            };
            products.ForEach(p => context.Products.Add(p));

            var planCategory1 = new Category { CategoryName = "New_voice_12m_and_24m" };
            var planCategory2 = new Category { CategoryName = "Sim_Only" };
            var plans = new List<Plan>
            {
                new Plan{PlanName = "Vodafone $40 24M-ON",OfferCode = "AU11044",Category = planCategory1},
                new Plan{PlanName = "Vodafone $50 24M-ON",OfferCode = "AU11094",Category = planCategory1},
                new Plan{PlanName = "Vodafone $30 SIMO-ON",OfferCode = "AU11056",Category = planCategory2},
                new Plan{PlanName = "Vodafone $45 SIMO",OfferCode = "AU11059",Category = planCategory2}
            };
            plans.ForEach(p => context.Plans.Add(p));

            var inventory1 = new Inventory { Dealer = dealers.Find(d => d.DealerName == "Swanston"), IMEI = "354380061969018", Product = products.Find(p => p.Name == "IPHONE 6 PLUS GOLD 16GB"), StockInDate = DateTime.Now,StockInBy = userManager.FindByName(name)};
            //var InvetoryChangeHistory1 = new InvetoryChangeHistory {ChangeDate = DateTime.Now, Inventory = inventory1,OperationType = InventoryOperationType.In};
            var inventory2 = new Inventory { Dealer = dealers.Find(d => d.DealerName == "Swanston"), IMEI = "356977061501200", Product = products.Find(p => p.Name == "IPHONE 6 SILVER 16GB"), StockInDate = DateTime.Now, StockInBy = userManager.FindByName(name) };
            //var InvetoryChangeHistory2 = new InvetoryChangeHistory { ChangeDate = DateTime.Now, Inventory = inventory2, OperationType = InventoryOperationType.In };

            //var InvetoryChangeHistoryList = new List<InvetoryChangeHistory>
            //{
            //    InvetoryChangeHistory1,
            //    InvetoryChangeHistory2
            //};
            //InvetoryChangeHistoryList.ForEach(i => context.InvetoryChangeHistories.Add(i));

            var inventories = new List<Inventory>
            {
                inventory1,
                inventory2
            };
            inventories.ForEach(i => context.Inventories.Add(i));
            #endregion

        }
    }
}

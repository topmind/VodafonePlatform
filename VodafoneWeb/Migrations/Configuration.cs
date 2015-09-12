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
            const string password = "alllink2Mel";
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

            //#region InitializeApplicationData

            var dealer1 = context.Dealers.FirstOrDefault(d => d.DealerName.Equals("Burwood"));
            if (dealer1 == null)
            {
                dealer1 = new Dealer { DealerName = "Burwood", DealerCode = "IDC167" };
                context.Dealers.Add(dealer1);
            }

            var dealer2 = context.Dealers.FirstOrDefault(d => d.DealerName.Equals("Carlton"));
            if (dealer2 == null)
            {
                dealer2 = new Dealer { DealerName = "Carlton", DealerCode = "IDC141" };
                context.Dealers.Add(dealer2);
            }

            var dealer3 = context.Dealers.FirstOrDefault(d => d.DealerName.Equals("Malvern"));
            if (dealer3 == null)
            {
                dealer3 = new Dealer { DealerName = "Malvern", DealerCode = "IDC052" };
                context.Dealers.Add(dealer3);
            }


            //var dealers = new List<Dealer>
            //{
            //    new Dealer {DealerName = "Burwood", DealerCode = "IDC167"},
            //    new Dealer {DealerName = "Carlton", DealerCode = "IDC141"},
            //    new Dealer {DealerName = "Malvern", DealerCode = "IDC052"}
            //};
            //dealers.ForEach(d => context.Dealers.Add(d));
            //var productCategory1 = context.ProductCategories.FirstOrDefault(p => p.ProductCategoryName.Equals("IPHONE"));
            //{
            //    if (productCategory1 == null)
            //    {
            //        productCategory1 = new ProductCategory { ProductCategoryName = "IPHONE" };
            //        context.ProductCategories.Add(productCategory1);
            //    }
            //}

            //var productCategory2 = context.ProductCategories.FirstOrDefault(p => p.ProductCategoryName.Equals("SAMSUNG"));
            //{
            //    if (productCategory2 == null)
            //    {
            //        productCategory2 = new ProductCategory { ProductCategoryName = "SAMSUNG" };
            //        context.ProductCategories.Add(productCategory2);
            //    }
            //}


            //var product1 = context.Products.Where(p => p.Name.Equals("IPHONE 6 PLUS GOLD 16GB")).FirstOrDefault(p => p.ProductCategory.ProductCategoryName.Equals(productCategory1.ProductCategoryName));
            //if (product1 == null)
            //{
            //    product1 = new Product { Name = "IPHONE 6 PLUS GOLD 16GB", ProductCategory = productCategory1 };
            //    context.Products.Add(product1);
            //}

            //var product2 = context.Products.Where(p => p.Name.Equals("IPHONE 6 SILVER 16GB")).FirstOrDefault(p => p.ProductCategory.ProductCategoryName.Equals(productCategory1.ProductCategoryName));
            //if (product2 == null)
            //{
            //    product2 = new Product { Name = "IPHONE 6 SILVER 16GB", ProductCategory = productCategory1 };
            //    context.Products.Add(product2);
            //}

            //var product3 = context.Products.Where(p => p.Name.Equals("SAMSUNG GALAXY S4 BLACK")).FirstOrDefault(p => p.ProductCategory.ProductCategoryName.Equals(productCategory2.ProductCategoryName));
            //if (product3 == null)
            //{
            //    product3 = new Product { Name = "SAMSUNG GALAXY S4 BLACK", ProductCategory = productCategory2 };
            //    context.Products.Add(product3);
            //}

            //var product4 = context.Products.Where(p => p.Name.Equals("SAMSUNG GALAXY S6 EDGE WHITE 32GB")).FirstOrDefault(p => p.ProductCategory.ProductCategoryName.Equals(productCategory2.ProductCategoryName));
            //if (product4 == null)
            //{
            //    product4 = new Product { Name = "SAMSUNG GALAXY S6 EDGE WHITE 32GB", ProductCategory = productCategory2 };
            //    context.Products.Add(product4);
            //}


            ////var products = new List<Product>
            ////{
            ////    new Product {Name="IPHONE 6 PLUS GOLD 16GB"},
            ////    new Product {Name = "IPHONE 6 SILVER 16GB"}
            ////};
            ////products.ForEach(p => context.Products.Add(p));


            var planCategory0 = context.Categories.FirstOrDefault(c => c.CategoryName.Equals("Outright"));
            if (planCategory0 == null)
            {
                planCategory0 = new Category { CategoryName = "Outright" };
                context.Categories.Add(planCategory0);
            }

            //var planCategory1 = context.Categories.FirstOrDefault(c => c.CategoryName.Equals("New_voice_12m_and_24m"));
            //if (planCategory1 == null)
            //{
            //    planCategory1 = new Category { CategoryName = "New_voice_12m_and_24m" };
            //    context.Categories.Add(planCategory1);
            //}

            //var planCategory2 = context.Categories.FirstOrDefault(c => c.CategoryName.Equals("Sim_Only"));
            //if (planCategory2 == null)
            //{
            //    planCategory2 = new Category { CategoryName = "Sim_Only" };
            //    context.Categories.Add(planCategory2);
            //}

            var plan0 = context.Plans.Where(p => p.PlanName.Equals("Outright")).FirstOrDefault(p => p.Category.CategoryName.Equals(planCategory0.CategoryName));
            if (plan0 == null)
            {
                plan0 = new Plan { PlanName = "Outright", Category = planCategory0 };
                context.Plans.Add(plan0);
            }

            //var plan1 = context.Plans.Where(p => p.PlanName.Equals("Vodafone $40 24M-ON")).FirstOrDefault(p => p.Category.CategoryName.Equals(planCategory1.CategoryName));
            //if (plan1 == null)
            //{
            //    plan1 = new Plan { PlanName = "Vodafone $40 24M-ON", OfferCode = "AU11044", Category = planCategory1 };
            //    context.Plans.Add(plan1);
            //}

            //var plan2 = context.Plans.Where(p => p.PlanName.Equals("Vodafone $50 24M-ON")).FirstOrDefault(p => p.Category.CategoryName.Equals(planCategory1.CategoryName));
            //if (plan2 == null)
            //{
            //    plan2 = new Plan { PlanName = "Vodafone $50 24M-ON", OfferCode = "AU11094", Category = planCategory1 };
            //    context.Plans.Add(plan2);
            //}

            //var plan3 = context.Plans.Where(p => p.PlanName.Equals("Vodafone $30 SIMO-ON")).FirstOrDefault(p => p.Category.CategoryName.Equals(planCategory2.CategoryName));
            //if (plan3 == null)
            //{
            //    plan3 = new Plan { PlanName = "Vodafone $30 SIMO-ON", OfferCode = "AU11056", Category = planCategory2 };
            //    context.Plans.Add(plan3);
            //}

            //var plan4 = context.Plans.Where(p => p.PlanName.Equals("Vodafone $45 SIMO")).FirstOrDefault(p => p.Category.CategoryName.Equals(planCategory2.CategoryName));
            //if (plan4 == null)
            //{
            //    plan4 = new Plan { PlanName = "Vodafone $45 SIMO", OfferCode = "AU11059", Category = planCategory2 };
            //    context.Plans.Add(plan4);
            //}




            ////var planCategory1 = new Category { CategoryName = "New_voice_12m_and_24m" };
            ////var planCategory2 = new Category { CategoryName = "Sim_Only" };
            ////var plans = new List<Plan>
            ////{
            ////    new Plan{PlanName = "Vodafone $40 24M-ON",OfferCode = "AU11044",Category = planCategory1},
            ////    new Plan{PlanName = "Vodafone $50 24M-ON",OfferCode = "AU11094",Category = planCategory1},
            ////    new Plan{PlanName = "Vodafone $30 SIMO-ON",OfferCode = "AU11056",Category = planCategory2},
            ////    new Plan{PlanName = "Vodafone $45 SIMO",OfferCode = "AU11059",Category = planCategory2}
            ////};
            ////plans.ForEach(p => context.Plans.Add(p));

            //var inventory1 = context.Inventories.FirstOrDefault(i => i.IMEI.Equals("354380061969018"));
            //if (inventory1 == null)
            //{
            //    //inventory1 = new Inventory { Dealer = context.Dealers.FirstOrDefault(d => d.DealerName == "Carlton"), IMEI = "354380061969001", Product = context.Products.FirstOrDefault(p => p.Name == "IPHONE 6 PLUS GOLD 16GB"), StockInDate = DateTime.Now, StockInBy = userManager.FindByName(name), Type = InventoryType.Standard };

            //    inventory1 = new Inventory { Dealer = dealer1, IMEI = "354380061969018", Product = product1, StockInDate = DateTime.Now, StockInBy = userManager.FindByName(name), Type = InventoryType.Standard };
            //    context.Inventories.Add(inventory1);
            //}

            //var inventory2 = context.Inventories.FirstOrDefault(i => i.IMEI.Equals("356977061501200"));
            //if (inventory2 == null)
            //{
            //    //inventory2 = new Inventory { Dealer = context.Dealers.FirstOrDefault(d => d.DealerName == "Carlton"), IMEI = "356977061501200", Product = context.Products.FirstOrDefault(p => p.Name == "IPHONE 6 SILVER 16GB"), StockInDate = DateTime.Now, StockInBy = userManager.FindByName(name), Type = InventoryType.Standard };

            //    inventory2 = new Inventory { Dealer = dealer1, IMEI = "356977061501200", Product = product2, StockInDate = DateTime.Now, StockInBy = userManager.FindByName(name), Type = InventoryType.Standard };
            //    context.Inventories.Add(inventory2);
            //}


            ////var inventory1 = new Inventory { Dealer = dealers.Find(d => d.DealerName == "Carlton"), IMEI = "354380061969001", Product = products.Find(p => p.Name == "IPHONE 6 PLUS GOLD 16GB"), StockInDate = DateTime.Now, StockInBy = userManager.FindByName(name), Type = InventoryType.Standard};
            //////var InvetoryChangeHistory1 = new InvetoryChangeHistory {ChangeDate = DateTime.Now, Inventory = inventory1,OperationType = InventoryOperationType.In};
            ////var inventory2 = new Inventory { Dealer = dealers.Find(d => d.DealerName == "Carlton"), IMEI = "356977061501002", Product = products.Find(p => p.Name == "IPHONE 6 SILVER 16GB"), StockInDate = DateTime.Now, StockInBy = userManager.FindByName(name), Type = InventoryType.Standard };
            //////var InvetoryChangeHistory2 = new InvetoryChangeHistory { ChangeDate = DateTime.Now, Inventory = inventory2, OperationType = InventoryOperationType.In };

            //////var InvetoryChangeHistoryList = new List<InvetoryChangeHistory>
            //////{
            //////    InvetoryChangeHistory1,
            //////    InvetoryChangeHistory2
            //////};
            //////InvetoryChangeHistoryList.ForEach(i => context.InvetoryChangeHistories.Add(i));

            ////var inventories = new List<Inventory>
            ////{
            ////    inventory1,
            ////    inventory2
            ////};
            ////inventories.ForEach(i => context.Inventories.Add(i));
            //#endregion

        }
    }
}

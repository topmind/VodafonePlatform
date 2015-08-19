﻿using System;
﻿using System.Collections;
﻿using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
﻿using System.Threading.Tasks;
﻿using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
﻿using Microsoft.AspNet.Identity;
﻿using VodafoneWeb.Models;
//﻿using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace VodafoneWeb.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewData["categories"] = db.Categories;
            ViewData["plans"] = db.Plans;
            ViewData["users"] = db.Users.Select(u => new SalesUserViewModule {UserId = u.Id, Username = u.UserName});
            ViewData["dealers"] = db.Dealers.Select(d => new DealerViewModel() { DealerId = d.DealerId, DealerCode = d.DealerCode, DealerName = d.DealerName});
            ViewData["products"] = db.Products.Select(p => new ProductViewModel(){ProductId = p.ID, ProductName = p.Name});
            return View();
        }

        public ActionResult Sales_Read([DataSourceRequest]DataSourceRequest request)
        {
            //IQueryable<SalesTransaction> salestransactions = db.SalesTransactions;
            //ICollection<SalesViewModel> salesViewModels = SalesModelToSalesViewModel();
            //DataSourceResult result = salesViewModels.ToDataSourceResult(request, saleViewModel => new
            //{
            //    ID = saleViewModel.ID,
            //    LastName = saleViewModel.LastName,
            //    FirstName = saleViewModel.FirstName,
            //    MobileNumber = saleViewModel.MobileNumber,
            //    Pin = saleViewModel.Pin,
            //    SalesEmployee = saleViewModel.SalesEmployee,
            //    IMEI = saleViewModel.IMEI,
            //    Category = new SalesCategoryViewModule()
            //    {
            //        CategoryID = saleViewModel.Category.CategoryID,
            //        CategoryName = saleViewModel.Category.CategoryName
            //    },
            //    Plan = new SalesPlanViewModule()
            //    {
            //        PlanID = saleViewModel.Plan.PlanID,
            //        PlanName = saleViewModel.Plan.PlanName
            //    }
            //});
            IEnumerable<SalesViewModel> data = db.SalesTransactions
                .Select(salestransaction => new SalesViewModel
                {
                    ID = salestransaction.ID,
                    LastName = salestransaction.LastName,
                    FirstName = salestransaction.FirstName,
                    MobileNumber = salestransaction.MobileNumber,
                    Pin = salestransaction.Pin,
                    OrderNumber = salestransaction.OrderNumber,
                    PortinNumber = salestransaction.PortinNumber,
                    PlanId = salestransaction.PlanId,
                    Plan = new SalesPlanViewModule
                    {
                        PlanID = salestransaction.LinkedPlan.PlanId,
                        PlanName = salestransaction.LinkedPlan.PlanName
                    },
                    CategoryId = salestransaction.LinkedPlan.CategoryID,
                    Category = new SalesCategoryViewModule
                    {
                        CategoryID = salestransaction.LinkedPlan.CategoryID,
                        CategoryName = salestransaction.LinkedPlan.Category.CategoryName
                    },
                    UserId = salestransaction.UserId,
                    User = new SalesUserViewModule
                    {
                        UserId = salestransaction.User.Id,
                        Username = salestransaction.User.UserName
                    },
                    DealerId = salestransaction.DealerId,
                    Dealer = new DealerViewModel
                    {
                        DealerId = salestransaction.Dealer.DealerId, 
                        DealerCode = salestransaction.Dealer.DealerCode,
                        DealerName = salestransaction.Dealer.DealerName
                    },
                    InventoryId = salestransaction.InventoryId,
                    Inventory = new SalesInventoryViewModel
                    {
                        InventoryId = salestransaction.Inventory.ID,
                        IMEI = salestransaction.Inventory.IMEI
                    },
                    IMEI = salestransaction.Inventory.IMEI,
                    ProductId = salestransaction.Inventory.Product.ID,
                    Product = new ProductViewModel
                    {
                        ProductId = salestransaction.Inventory.Product.ID,
                        ProductName = salestransaction.Inventory.Product.Name
                    },
                    CreateDateTime = salestransaction.CreateDateTime,
                    RefferA = salestransaction.RefferA,
                    RefferB = salestransaction.RefferB,
                    Gift = salestransaction.Gift,
                    Note = salestransaction.Note
                });

            data = data.Where(d => d.DealerId == HelperMethods.GetDealerId());
                //.Where(salestransaction => salestransaction.DealerId == HelperMethods.GetDealerId());

            DataSourceResult result = data.ToDataSourceResult(request);

            return Json(result);
        }

        //private ICollection<SalesViewModel> SalesModelToSalesViewModel()
        //{
        //    IQueryable<SalesTransaction> model = db.SalesTransactions;

        //    ICollection<SalesViewModel> viewModel = new List<SalesViewModel>();

        //    foreach (var sales in model)
        //    {
        //        SalesViewModel vm = new SalesViewModel();
        //        vm.ID = sales.ID;
        //        vm.LastName = sales.LastName;
        //        vm.FirstName = sales.FirstName;
        //        vm.MobileNumber = sales.MobileNumber;
        //        vm.Pin = sales.Pin;
               

        //        viewModel.Add(vm);
        //    }

        //    return viewModel; 
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Sales_Create([DataSourceRequest] DataSourceRequest request, SalesViewModel salesViewModel)
        {
            if (salesViewModel != null && ModelState.IsValid)
            {
                salesViewModel.UserId = User.Identity.GetUserId();
                if(HelperMethods.GetDealerId().HasValue)
                    salesViewModel.DealerId = HelperMethods.GetDealerId().GetValueOrDefault();
                else
                {
                    return HttpNotFound("No Current Dealer Found");
                }

                salesViewModel.CreateDateTime = DateTime.Now;

                SalesTransaction data = new SalesTransaction
                {
                    LastName = salesViewModel.LastName,
                    FirstName = salesViewModel.FirstName,
                    MobileNumber = salesViewModel.MobileNumber,
                    Pin = salesViewModel.Pin,
                    OrderNumber = salesViewModel.OrderNumber,
                    PortinNumber = salesViewModel.PortinNumber,
                    PlanId = salesViewModel.PlanId,
                    UserId = salesViewModel.UserId,
                    DealerId = salesViewModel.DealerId,
                    InventoryId = salesViewModel.InventoryId,
                    CreateDateTime = salesViewModel.CreateDateTime,
                    RefferA = salesViewModel.RefferA,
                    RefferB = salesViewModel.RefferB,
                    Note = salesViewModel.Note
                };

                db.SalesTransactions.Add(data);
                await db.SaveChangesAsync();
            }
            //salesTransaction.LinkedPlan = db.Plans.FirstOrDefault(p => p.PlanId == salesTransaction.PlanId);
            //await db.SaveChangesAsync();
            return Json(new[] { salesViewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Sales_Update([DataSourceRequest] DataSourceRequest request, SalesTransaction salesTransaction)
        {
            if (salesTransaction != null && ModelState.IsValid)
            {
                db.Entry(salesTransaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return Json(new[] { salesTransaction }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetCategories()
        {
            var data = db.Categories;

            var list = new List<Category>();
            //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
            //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

            foreach (Category cat in data)
            {
                list.Add(new Category { CategoryId = cat.CategoryId, CategoryName = cat.CategoryName });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlans(int categoryId)
        {
            var data = db.Plans.Where(p => p.CategoryID == categoryId);

            var list = new List<Plan>();

            foreach (Plan plan in data)
            {
                list.Add(new Plan { PlanId = plan.PlanId, PlanName = plan.PlanName });
            }


            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProducts(int inventoryId)
        {
            var firstOrDefault = db.Inventories.FirstOrDefault(i => i.ID == inventoryId);
            if (firstOrDefault != null)
            {
                var data = firstOrDefault.Product;

                if(data == null)
                    return new JsonResult();

                var list = new List<ProductViewModel>();
                //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
                //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

                //foreach (Product prd in data)
                //{
                list.Add(new ProductViewModel { ProductId = data.ID, ProductName = data.Name });
                //}
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return new JsonResult();
        }


        public JsonResult GetIMEI()
        {
            var data = db.Inventories;

            var list = new List<SalesInventoryViewModel>();
            //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
            //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

            foreach (Inventory inventory in data)
            {
                list.Add(new SalesInventoryViewModel { InventoryId = inventory.ID, IMEI = inventory.IMEI });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        

        //public JsonResult GetPlans()
        //{
        //    var data = db.Plans;

        //    var list = new List<Plan>();

        //    foreach (Plan plan in data)
        //    {
        //        list.Add(new Plan { PlanId = plan.PlanId, PlanName = plan.PlanName });
        //    }


        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

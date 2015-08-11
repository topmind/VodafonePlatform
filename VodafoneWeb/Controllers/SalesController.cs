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
﻿using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace VodafoneWeb.Controllers
{
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewData["categories"] = db.Categories;
            ViewData["plans"] = db.Plans;
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
                    }

                });

            DataSourceResult result = data.ToDataSourceResult(request);

            return Json(result);
        }

        private ICollection<SalesViewModel> SalesModelToSalesViewModel()
        {
            IQueryable<SalesTransaction> model = db.SalesTransactions;

            ICollection<SalesViewModel> viewModel = new List<SalesViewModel>();

            foreach (var sales in model)
            {
                SalesViewModel vm = new SalesViewModel();
                vm.ID = sales.ID;
                vm.LastName = sales.LastName;
                vm.FirstName = sales.FirstName;
                vm.MobileNumber = sales.MobileNumber;
                vm.Pin = sales.Pin;
               

                viewModel.Add(vm);
            }

            return viewModel; 
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Sales_Create([DataSourceRequest] DataSourceRequest request, SalesTransaction salesTransaction)
        {
            if (salesTransaction != null && ModelState.IsValid)
            {
                db.SalesTransactions.Add(salesTransaction);
                //await db.SaveChangesAsync();
            }
            await db.SaveChangesAsync();

            return Json(new[] { salesTransaction }.ToDataSourceResult(request, ModelState));
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

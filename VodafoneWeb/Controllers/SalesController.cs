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
            ViewData["productCategories"] = db.ProductCategories;
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
                    Type = salestransaction.Inventory != null ? salestransaction.Inventory.Type:null,
                    InventoryId = salestransaction.InventoryId,
                    Inventory = salestransaction.Inventory != null ? new SalesInventoryViewModel
                    {
                        Type = salestransaction.Inventory.Type,
                        InventoryId = salestransaction.Inventory.InventoryId,
                        IMEI = salestransaction.Inventory.IMEI,
                        Status = salestransaction.Inventory.Status
                    } : null,
                    IMEI = salestransaction.Inventory != null? salestransaction.Inventory.IMEI : null,
                    ProductCategoryId = salestransaction.Product.ProductCategoryId,
                    ProductId = salestransaction.ProductId,
                    Product = new ProductViewModel
                    {
                        ProductId = salestransaction.ProductId,
                        ProductName = salestransaction.Product.Name
                    },
                    CreateDateTime = salestransaction.CreateDateTime,
                    RefferA = salestransaction.RefferA,
                    RefferB = salestransaction.RefferB,
                    Gift = salestransaction.Gift,
                    Audit = salestransaction.Audit,
                    Note = salestransaction.Note,
                    IsChanged = salestransaction.IsChanged
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
                    Audit = salesViewModel.Audit,
                    Note = salesViewModel.Note,
                    ProductId = salesViewModel.ProductId
                };

                db.SalesTransactions.Add(data);

                if (data.InventoryId.HasValue)
                {
                    int id = data.InventoryId.Value;

                    Inventory inventory = await db.Inventories.FindAsync(id);
                    if (inventory != null)
                    {
                        InventoryOperationType oldType = inventory.Status;
                        inventory.Status = InventoryOperationType.Out;
                        inventory.StockOutById = data.UserId;
                        inventory.StockOutDate = DateTime.Now;
                        salesViewModel.IMEI = inventory.IMEI;

                        //add change record to history
                        InvetoryChangeHistory changeHistory = new InvetoryChangeHistory();
                        changeHistory.IMEI = inventory.IMEI;
                        changeHistory.ChangeDate = inventory.StockOutDate.Value;
                        changeHistory.OldOperationType = oldType;
                        changeHistory.NewOperationType = InventoryOperationType.Out;
                        changeHistory.OperatedByEmployeeID = inventory.StockOutById;
                        changeHistory.Sales = data;
                        db.InvetoryChangeHistories.Add(changeHistory);
                    }
                }
                await db.SaveChangesAsync();
                salesViewModel.ID = data.ID;

                //return RedirectToAction("StockOut", "Inventory", salesViewModel.InventoryId);
            }
            //salesTransaction.LinkedPlan = db.Plans.FirstOrDefault(p => p.PlanId == salesTransaction.PlanId);
            //await db.SaveChangesAsync();
            return Json(new[] { salesViewModel }.ToDataSourceResult(request, ModelState));
            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Sales_Update([DataSourceRequest] DataSourceRequest request, SalesViewModel salesViewModel)
        {
            if (salesViewModel != null && ModelState.IsValid)
            {
                SalesTransaction data = db.SalesTransactions.Find(salesViewModel.ID);

                data.LastName = salesViewModel.LastName;
                data.FirstName = salesViewModel.FirstName;
                data.MobileNumber = salesViewModel.MobileNumber;
                data.Pin = salesViewModel.Pin;
                data.OrderNumber = salesViewModel.OrderNumber;
                data.PortinNumber = salesViewModel.PortinNumber;
                data.PlanId = salesViewModel.PlanId;
                data.UserId = salesViewModel.UserId;
                data.DealerId = salesViewModel.DealerId;

                if (data.InventoryId != salesViewModel.InventoryId)
                {
                    if (data.InventoryId.HasValue)
                    {
                        Inventory oldInventory = await db.Inventories.FindAsync(data.InventoryId);

                        if (oldInventory != null)
                        {
                            oldInventory.Status = InventoryOperationType.In;
                            oldInventory.StockOutById = null;
                            oldInventory.StockOutDate = null;

                            db.Entry(oldInventory).State = EntityState.Modified;

                            //add change record to history
                            InvetoryChangeHistory changeHistory = new InvetoryChangeHistory();
                            changeHistory.IMEI = oldInventory.IMEI;
                            changeHistory.ChangeDate = DateTime.Now;
                            changeHistory.OldOperationType = InventoryOperationType.Out;
                            changeHistory.NewOperationType = InventoryOperationType.In;
                            changeHistory.OperatedByEmployeeID = salesViewModel.UserId;
                            changeHistory.Sales = null;
                            db.InvetoryChangeHistories.Add(changeHistory);
                        }
                    }

                    if (salesViewModel.InventoryId.HasValue)
                    {
                        int id = salesViewModel.InventoryId.Value;

                        Inventory inventory = await db.Inventories.FindAsync(id);
                        if (inventory != null)
                        {
                            InventoryOperationType oldType = inventory.Status;
                            inventory.Status = InventoryOperationType.Out;
                            inventory.StockOutById = data.UserId;
                            inventory.StockOutDate = DateTime.Now;
                            salesViewModel.IMEI = inventory.IMEI;

                            //add change record to history
                            InvetoryChangeHistory changeHistory = new InvetoryChangeHistory();
                            changeHistory.IMEI = inventory.IMEI;
                            changeHistory.ChangeDate = inventory.StockOutDate.Value;
                            changeHistory.OldOperationType = oldType;
                            changeHistory.NewOperationType = InventoryOperationType.Out;
                            changeHistory.OperatedByEmployeeID = inventory.StockOutById;
                            changeHistory.Sales = data;
                            db.InvetoryChangeHistories.Add(changeHistory);
                        }
                    }
                }

                data.InventoryId = salesViewModel.InventoryId;
                data.CreateDateTime = salesViewModel.CreateDateTime;
                data.RefferA = salesViewModel.RefferA;
                data.RefferB = salesViewModel.RefferB;
                data.Audit = salesViewModel.Audit;
                data.Note = salesViewModel.Note;
                data.ProductId = salesViewModel.ProductId;


                if (!User.IsInRole("Admin"))
                {
                    data.IsChanged = true;
                    salesViewModel.IsChanged = true;
                }

                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return Json(new[] { salesViewModel }.ToDataSourceResult(request, ModelState));
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

        public JsonResult GetProductCategories(int? inventoryId)
        {
            var list = new List<ProductCategory>();
            if (inventoryId.HasValue)
            {
                var firstOrDefault = db.Inventories.FirstOrDefault(i => i.InventoryId == inventoryId);

                if (firstOrDefault != null)
                {
                    var productdata = firstOrDefault.Product;

                    if (productdata == null)
                        return new JsonResult();


                    //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
                    //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

                    //foreach (Product prd in data)
                    //{
                    list.Add(new ProductCategory { ProductCategoryId = productdata.ProductCategoryId, ProductCategoryName = productdata.ProductCategory.ProductCategoryName });
                    //}
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
            
            
            var data = db.ProductCategories;

            
            //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
            //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

            foreach (ProductCategory cat in data)
            {
                list.Add(new ProductCategory { ProductCategoryId = cat.ProductCategoryId, ProductCategoryName = cat.ProductCategoryName });
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

        public JsonResult GetProducts(int? inventoryId, int? productCategoryId)
        {
            var list = new List<ProductViewModel>();
            if (inventoryId.HasValue)
            {
                var firstOrDefault = db.Inventories.FirstOrDefault(i => i.InventoryId == inventoryId);
                
                if (firstOrDefault != null)
                {
                    var data = firstOrDefault.Product;

                    if (data == null)
                        return new JsonResult();


                    //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
                    //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

                    //foreach (Product prd in data)
                    //{
                    list.Add(new ProductViewModel {ProductId = data.ID, ProductName = data.Name});
                    //}
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                //else
                //{
                //    var products = db.Products.Where(p => p.IsActive).ToList();
                //    if (products != null && products.Any())
                //    {
                //        foreach (Product product in products)
                //        {
                //            list.Add(new ProductViewModel { ProductId = product.ID, ProductName = product.Name });
                //        }
                //        return Json(list, JsonRequestBehavior.AllowGet);
                //    }
                //    return new JsonResult();
                //}
            }
            else if (productCategoryId.HasValue)
            {
                var products = db.Products.Where(p => p.IsActive).Where(p => p.ProductCategoryId == productCategoryId).ToList();
                if (products != null && products.Any())
                {
                    foreach (Product product in products)
                    {
                        list.Add(new ProductViewModel { ProductId = product.ID, ProductName = product.Name });
                    }
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                return new JsonResult();   
            }
            return new JsonResult();   
        }


        public JsonResult GetIMEIs(InventoryType type, int? inventoryId, string IMEI)
        {
            var list = new List<SalesInventoryViewModel>();
            var dealerId = HelperMethods.GetDealerId();

            if (dealerId != null)
            {
                int dId = dealerId.Value;
                var data =
                    db.Inventories.Where(i => i.DealerId == dId)
                        .Where(i => i.Status == InventoryOperationType.In || i.Status == InventoryOperationType.TransferIn)
                        .Where(i => i.Type == type);
                

                
                //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
                //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });



                if (data.Any())
                    foreach (Inventory inventory in data)
                    {
                        list.Add(new SalesInventoryViewModel
                        {
                            InventoryId = inventory.InventoryId,
                            IMEI = inventory.IMEI
                        });
                    }
                if (inventoryId.HasValue && !string.IsNullOrEmpty(IMEI))
                {
                    list.Add(new SalesInventoryViewModel
                    {
                        InventoryId = inventoryId.Value,
                        IMEI = IMEI
                    });
                }
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

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            fileName = HelperMethods.FormatFileName(fileName); 
            
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

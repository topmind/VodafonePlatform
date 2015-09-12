﻿using System;
using System.Collections.Generic;
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

namespace VodafoneWeb.Controllers
{
     [Authorize]
    public class InventoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewData["users"] = db.Users.Select(u => new SalesUserViewModule { UserId = u.Id, Username = u.UserName });
            ViewData["dealers"] = db.Dealers.Select(d => new DealerViewModel() { DealerId = d.DealerId, DealerCode = d.DealerCode, DealerName = d.DealerName });
            ViewData["productCategories"] = db.ProductCategories;//.Select(p => new SalesProductCategoryViewModel { ProductCategoryID = p.ProductCategoryId, ProductCategoryName = p.ProductCategoryName});
            ViewData["products"] = db.Products.Select(p => new ProductViewModel() { ProductId = p.ID, ProductName = p.Name });
            return View();
        }

        public ActionResult Inventories_Read([DataSourceRequest]DataSourceRequest request)
        {
            //IQueryable<Inventory> inventories = db.Inventories;
            //DataSourceResult result = inventories.ToDataSourceResult(request, inventory => new {
            //    ID = inventory.ID,
            //    IMEI = inventory.IMEI,
            //    InStock = inventory.Status
            //});

            IEnumerable<InventoryViewModel> data = db.Inventories
                .Select(inventory => new InventoryViewModel
                {
                    ID = inventory.InventoryId,
                    StockInDate = inventory.StockInDate,
                    StockInUserId = inventory.StockInBy.Id,
                    StockInBy = inventory.StockInBy,
                    DealerId = inventory.Dealer.DealerId,
                    Dealer = new DealerViewModel
                    {
                        DealerId = inventory.Dealer.DealerId,
                        DealerCode = inventory.Dealer.DealerCode,
                        DealerName = inventory.Dealer.DealerName
                    },
                    Status = inventory.Status,
                    Type = inventory.Type ?? InventoryType.Standard,
                    IMEI = inventory.IMEI,
                    ProductCategoryId = inventory.Product.ProductCategoryId,
                    ProductCategory = new SalesProductCategoryViewModel
                    {
                        ProductCategoryID = inventory.Product.ProductCategoryId,
                        ProductCategoryName = inventory.Product.ProductCategory.ProductCategoryName
                    },
                    ProductId = inventory.Product.ID,
                    Product = new ProductViewModel
                    {
                        ProductId = inventory.Product.ID,
                        ProductName = inventory.Product.Name
                    },
                    StockOutDate = inventory.StockOutDate,
                    StockOutUserId = inventory.StockOutBy.Id,
                    StockOutBy = inventory.StockOutBy,
                    PurchasedFrom = inventory.PurchasedFrom,
                    PurchasedById = inventory.PurchasedById,
                    PurchasedBy = inventory.PurchasedBy,
                    DefferCode = inventory.DefferCode,
                    DefferName = inventory.DefferName,
                    DefferOrderNo = inventory.DefferOrderNo,
                    Note = inventory.Note
                });

            data = data.Where(d => d.DealerId == HelperMethods.GetDealerId());
            //.Where(salestransaction => salestransaction.DealerId == HelperMethods.GetDealerId());

            DataSourceResult result = data.ToDataSourceResult(request);

            return Json(result);
        }

        public ActionResult Inventories50D_Read([DataSourceRequest]DataSourceRequest request)
        {
            //IQueryable<Inventory> inventories = db.Inventories;
            //DataSourceResult result = inventories.ToDataSourceResult(request, inventory => new {
            //    ID = inventory.ID,
            //    IMEI = inventory.IMEI,
            //    InStock = inventory.Status
            //});

            IEnumerable<InventoryViewModel> data = db.Inventories
                .Select(inventory => new InventoryViewModel
                {
                    ID = inventory.InventoryId,
                    StockInDate = inventory.StockInDate,
                    StockInUserId = inventory.StockInBy.Id,
                    StockInBy = inventory.StockInBy,
                    DealerId = inventory.Dealer.DealerId,
                    Dealer = new DealerViewModel
                    {
                        DealerId = inventory.Dealer.DealerId,
                        DealerCode = inventory.Dealer.DealerCode,
                        DealerName = inventory.Dealer.DealerName
                    },
                    Status = inventory.Status,
                    Type = inventory.Type ?? InventoryType.Standard,
                    IMEI = inventory.IMEI,
                    ProductId = inventory.Product.ID,
                    Product = new ProductViewModel
                    {
                        ProductId = inventory.Product.ID,
                        ProductName = inventory.Product.Name
                    },
                    PurchasedFrom = inventory.PurchasedFrom,
                    PurchasedById = inventory.PurchasedById,
                    PurchasedBy = inventory.PurchasedBy,
                    DefferCode = inventory.DefferCode,
                    DefferName = inventory.DefferName,
                    DefferOrderNo = inventory.DefferOrderNo,
                    Note = inventory.Note
                });

            data = data.Where(d => d.DealerId == HelperMethods.GetDealerId())
                .Where(i => i.Status == InventoryOperationType.In || i.Status == InventoryOperationType.TransferIn)
                .Where(i => i.StockInDate < DateTime.Now.AddDays(-50));
            //.Where(salestransaction => salestransaction.DealerId == HelperMethods.GetDealerId());

            DataSourceResult result = data.ToDataSourceResult(request);

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Inventory_Create([DataSourceRequest] DataSourceRequest request, InventoryViewModel inventoryViewModel)
        {
            //if (inventory != null && ModelState.IsValid)
            //{
            //    db.Inventories.Add(inventory);
            //    //await db.SaveChangesAsync();
            //}

            ////var invetoryChangeHistory = new InvetoryChangeHistory
            ////{
            ////    ChangeDate = DateTime.Now,
            ////    Inventory = inventory,
            ////    OperatedByEmployeeID = User.Identity.GetUserId(),
            ////    OperationType = InventoryOperationType.In
            ////};

            ////db.InvetoryChangeHistories.Add(invetoryChangeHistory);
            //await db.SaveChangesAsync();

            //return Json(new[] { inventory }.ToDataSourceResult(request, ModelState));




            if (inventoryViewModel != null && ModelState.IsValid)
            {
                inventoryViewModel.StockInUserId = User.Identity.GetUserId();
                if (HelperMethods.GetDealerId().HasValue)
                    inventoryViewModel.DealerId = HelperMethods.GetDealerId().GetValueOrDefault();
                else
                {
                    //return HttpNotFound("No Current Dealer Found");
                }

                inventoryViewModel.StockInDate = DateTime.Now;

                Inventory data = new Inventory
                {
                    StockInById = inventoryViewModel.StockInUserId,
                    StockInDate = inventoryViewModel.StockInDate,
                    DealerId = inventoryViewModel.DealerId,
                    ProductId = inventoryViewModel.ProductId,
                    IMEI = inventoryViewModel.IMEI,
                    Type = inventoryViewModel.Type,
                    Note = inventoryViewModel.Note,
                    PurchasedFrom = string.IsNullOrEmpty(inventoryViewModel.PurchasedFrom) ? null:inventoryViewModel.PurchasedFrom,
                    PurchasedById = inventoryViewModel.PurchasedById,
                    DefferCode = string.IsNullOrEmpty(inventoryViewModel.DefferCode) ? null : inventoryViewModel.DefferCode,
                    DefferName = string.IsNullOrEmpty(inventoryViewModel.DefferName) ? null : inventoryViewModel.DefferName,
                    DefferOrderNo = string.IsNullOrEmpty(inventoryViewModel.DefferOrderNo) ? null : inventoryViewModel.DefferOrderNo
                };

                db.Inventories.Add(data);


                //add change record to history
                InvetoryChangeHistory changeHistory2 = new InvetoryChangeHistory();
                changeHistory2.IMEI = data.IMEI;
                changeHistory2.ChangeDate = data.StockInDate;
                changeHistory2.OldOperationType = null;
                changeHistory2.NewOperationType = InventoryOperationType.In;
                changeHistory2.OperatedByEmployeeID = data.StockInById;
                var result = await HelperMethods.GetCurrentDealer();
                changeHistory2.ToDealer = result.DealerName;
                db.InvetoryChangeHistories.Add(changeHistory2);

                await db.SaveChangesAsync();
                //db.SaveChanges();
                //inventoryViewModel.ID = data.InventoryId;
                await db.SaveChangesAsync();
                inventoryViewModel.ID = data.InventoryId;
            }
            //salesTransaction.LinkedPlan = db.Plans.FirstOrDefault(p => p.PlanId == salesTransaction.PlanId);
            //await db.SaveChangesAsync();
            return Json(new[] { inventoryViewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Inventory_Update([DataSourceRequest] DataSourceRequest request, InventoryViewModel inventoryViewModel)
        {
            if (inventoryViewModel != null && ModelState.IsValid)
            {
                Inventory data = db.Inventories.Find(inventoryViewModel.ID);

                //data.StockInById = inventoryViewModel.StockInUserId;
                //data.StockInDate = inventoryViewModel.StockInDate;
                //data.DealerId = inventoryViewModel.DealerId;
                List<SalesTransaction> sales = db.SalesTransactions.Where(s => s.InventoryId == inventoryViewModel.ID).ToList();

                if (sales.Count == 1)
                {
                    sales[0].ProductId = inventoryViewModel.ProductId;
                }


                data.ProductId = inventoryViewModel.ProductId;
                data.IMEI = inventoryViewModel.IMEI;
                data.Type = inventoryViewModel.Type;
                data.Note = inventoryViewModel.Note;
                if (data.Type == InventoryType.Purchased)
                {
                    data.PurchasedFrom = string.IsNullOrEmpty(inventoryViewModel.PurchasedFrom) ? null : inventoryViewModel.PurchasedFrom;
                    data.PurchasedById = inventoryViewModel.PurchasedById;
                    data.DefferCode = null;
                    data.DefferName = null;
                    data.DefferOrderNo = null;
                    inventoryViewModel.DefferCode = null;
                    inventoryViewModel.DefferName = null;
                    inventoryViewModel.DefferOrderNo = null;
                }
                else if (data.Type == InventoryType.Deffer)
                {
                    data.DefferCode = string.IsNullOrEmpty(inventoryViewModel.DefferCode) ? null : inventoryViewModel.DefferCode;
                    data.DefferName = string.IsNullOrEmpty(inventoryViewModel.DefferName) ? null : inventoryViewModel.DefferName;
                    data.DefferOrderNo = string.IsNullOrEmpty(inventoryViewModel.DefferOrderNo) ? null : inventoryViewModel.DefferOrderNo;
                    data.PurchasedFrom = null;
                    data.PurchasedById = null;
                    inventoryViewModel.PurchasedFrom = null;
                    inventoryViewModel.PurchasedById = null;
                }
                else
                {
                    data.PurchasedFrom = null;
                    data.PurchasedById = null;
                    data.DefferCode = null;
                    data.DefferName = null;
                    data.DefferOrderNo = null;
                    inventoryViewModel.PurchasedFrom = null;
                    inventoryViewModel.PurchasedById = null;
                    inventoryViewModel.DefferCode = null;
                    inventoryViewModel.DefferName = null;
                    inventoryViewModel.DefferOrderNo = null;
                }
                
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return Json(new[] { inventoryViewModel }.ToDataSourceResult(request, ModelState));
        }

         //public async Task<ActionResult> StockOut(int? id)
         //{
         //    if (id == null)
         //    {
         //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         //    }
         //    Inventory inventory = await db.Inventories.FindAsync(id);
         //    if (inventory == null)
         //    {
         //        return HttpNotFound();
         //    }
         //    return View(inventory);
         //}

         // POST: Dealers/Edit/5
         // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
         // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
         //[HttpPost]
         //[ValidateAntiForgeryToken]
         //public async Task<ActionResult> StockOut(Inventory inventory)
         //{
         //    if (ModelState.IsValid)
         //    {
         //        InventoryOperationType oldvType = inventory.Status;

         //        inventory.Status = InventoryOperationType.Out;
         //        inventory.StockOutById = User.Identity.GetUserId();
         //        inventory.StockOutDate = DateTime.Now;
         //        db.Entry(inventory).State = EntityState.Modified;

         //        //add change record to history
         //        InvetoryChangeHistory changeHistory = new InvetoryChangeHistory();
         //        changeHistory.IMEI = inventory.IMEI;
         //        changeHistory.ChangeDate = inventory.StockOutDate.Value;
         //        changeHistory.OldOperationType = oldvType;
         //        changeHistory.NewOperationType = InventoryOperationType.Out;
         //        changeHistory.OperatedByEmployeeID = inventory.StockOutById;
         //        db.InvetoryChangeHistories.Add(changeHistory);

         //        await db.SaveChangesAsync();
         //        return RedirectToAction("Index","Sales");
         //    }
         //    return RedirectToAction("Index", "Sales");
         //}

        public async Task<ActionResult> TransferTo(int? dealerId)
        {
            if (TempData.ContainsKey("InventoryId") && dealerId != null)
            {
                int? id = (int?)TempData["InventoryId"];
                if (id != null)
                {
                    var userId = User.Identity.GetUserId();
                    Dealer destDealer = await db.Dealers.FindAsync(dealerId);

                    Dealer currentDealer = await HelperMethods.GetCurrentDealer();
                    
                    Inventory inventory =  await db.Inventories.FindAsync(id);

                    string note = inventory.Note;
                    InventoryOperationType oldStatus = inventory.Status;
                    inventory.Note = " To " + destDealer.DealerName;
                    inventory.Status = InventoryOperationType.TransferOut;
                    inventory.StockOutById = userId;
                    inventory.StockOutDate = DateTime.Now;

                    db.Entry(inventory).State = EntityState.Modified;

                    //add change record to history
                    InvetoryChangeHistory changeHistory = new InvetoryChangeHistory();
                    changeHistory.IMEI = inventory.IMEI;
                    changeHistory.ChangeDate = inventory.StockOutDate.Value;
                    changeHistory.OldOperationType = oldStatus;
                    changeHistory.NewOperationType = InventoryOperationType.TransferOut;
                    changeHistory.OperatedByEmployeeID = inventory.StockOutById;
                    changeHistory.ToDealer = destDealer.DealerName;
                    db.InvetoryChangeHistories.Add(changeHistory);

                    Inventory destInventory = new Inventory
                    {
                        StockInById = userId,
                        StockInDate = inventory.StockInDate,
                        DealerId = dealerId.Value,
                        ProductId = inventory.ProductId,
                        IMEI = inventory.IMEI,
                        Status = InventoryOperationType.TransferIn,
                        Type = inventory.Type,
                        Note = " From " + currentDealer.DealerName,
                        PurchasedFrom = inventory.PurchasedFrom,
                        PurchasedById = inventory.PurchasedById,
                        DefferCode = inventory.DefferCode,
                        DefferName = inventory.DefferName,
                        DefferOrderNo = inventory.DefferOrderNo,
                        StockOutById = null,
                        StockOutDate = null
                    };
                    db.Inventories.Add(destInventory);

                    //add change record to history
                    InvetoryChangeHistory changeHistory2 = new InvetoryChangeHistory();
                    changeHistory2.IMEI = inventory.IMEI;
                    changeHistory2.ChangeDate = destInventory.StockInDate;
                    changeHistory2.OldOperationType = null;
                    changeHistory2.NewOperationType = InventoryOperationType.TransferIn;
                    changeHistory2.OperatedByEmployeeID = inventory.StockOutById;
                    changeHistory2.FromDealer = currentDealer.DealerName;
                    db.InvetoryChangeHistories.Add(changeHistory2);

                    await db.SaveChangesAsync();
                }
            }
 
            //db.Inventories.Find(dealerId)
            return RedirectToAction("Index");
        }

        //public JsonResult GetProducts()
        //{
        //    var data = db.Products.Where(p => p.IsActive.Equals(true));

        //    var list = new List<Product>();
        //    //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
        //    //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

        //    foreach (Product product in data)
        //    {
        //        list.Add(new Product { ID = product.ID, Name = product.Name });
        //    }
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetProductCategories()
        {
            var data = db.ProductCategories;

            var list = new List<ProductCategory>();
            ////list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
            ////list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

            foreach (ProductCategory cat in data)
            {
                list.Add(new ProductCategory { ProductCategoryId = cat.ProductCategoryId, ProductCategoryName = cat.ProductCategoryName });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProducts(int? productCategoryId)
        {
            var list = new List<ProductViewModel>();

            if (productCategoryId.HasValue)
            {
                var products = db.Products.Where(p => p.IsActive).Where(p => p.ProductCategoryId == productCategoryId).ToList();
                if (products.Any())
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

        public JsonResult GetUsers()
        {
            var data = db.Users;

            var list = new List<ApplicationUser>();
            //list.Add(new Category{CategoryId = 1, CategoryName = "Cat 1"});
            //list.Add(new Category { CategoryId = 2, CategoryName = "Cat 2" });

            foreach (ApplicationUser user in data)
            {
                list.Add(new ApplicationUser{ Id = user.Id, UserName = user.UserName });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

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

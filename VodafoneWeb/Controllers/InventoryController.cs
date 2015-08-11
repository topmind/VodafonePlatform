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
            return View();
        }

        public ActionResult Inventories_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Inventory> inventories = db.Inventories;
            DataSourceResult result = inventories.ToDataSourceResult(request, inventory => new {
                ID = inventory.ID,
                IMEI = inventory.IMEI,
                InStock = inventory.InStock
            });

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Inventory_Create([DataSourceRequest] DataSourceRequest request, Inventory inventory)
        {
            if (inventory != null && ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                //await db.SaveChangesAsync();
            }

            var invetoryChangeHistory = new InvetoryChangeHistory
            {
                ChangeDate = DateTime.Now,
                Inventory = inventory,
                OperatedByEmployeeID = User.Identity.GetUserId(),
                OperationType = InventoryOperationType.In
            };

            db.InvetoryChangeHistories.Add(invetoryChangeHistory);
            await db.SaveChangesAsync();

            return Json(new[] { inventory }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Inventory_Update([DataSourceRequest] DataSourceRequest request, Inventory inventory)
        {
            if (inventory != null && ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return Json(new[] { inventory }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

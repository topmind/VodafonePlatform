﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VodafoneWeb.Models;

namespace VodafoneWeb.Controllers
{
    public class IMEIHistoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewData["users"] = db.Users.Select(u => new SalesUserViewModule { UserId = u.Id, Username = u.UserName });
            return View();
        }

        public ActionResult InvetoryChangeHistories_Read([DataSourceRequest]DataSourceRequest request)
        {
            IEnumerable<InventoryChangeHistoryViewModel> data =
                db.InvetoryChangeHistories.Select(inventoryChangeHistory =>
                    new InventoryChangeHistoryViewModel
                    {
                        InvetoryChangeHistoryId = inventoryChangeHistory.InvetoryChangeHistoryId,
                        IMEI = inventoryChangeHistory.IMEI,
                        ChangeDate = inventoryChangeHistory.ChangeDate,
                        OldOperationType = inventoryChangeHistory.OldOperationType,
                        NewOperationType = inventoryChangeHistory.NewOperationType,
                        OperatedByEmployeeID = inventoryChangeHistory.OperatedByEmployeeID,
                        //SalesId = inventoryChangeHistory.SalesId,
                        //Sales = inventoryChangeHistory.Sales,
                        FromDealer = inventoryChangeHistory.FromDealer,
                        ToDealer = inventoryChangeHistory.ToDealer
                    });
            DataSourceResult result = data.ToDataSourceResult(request);
            return Json(result);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

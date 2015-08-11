using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VodafoneWeb.Models;

namespace VodafoneWeb.Services
{
    public class SalesService : IDisposable
    {
        private ApplicationDbContext _dbContext;

        public SalesService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //public IEnumerable<SalesViewModel> Read()
        //{
        //    return _dbContext.SalesTransactions.Select(sales => new SalesViewModel()
        //    {
        //        ID = sales.ID,
        //        LastName = sales.LastName,
        //        FirstName = sales.FirstName,
        //        MobileNumber = sales.MobileNumber,
        //        Pin = sales.Pin,
        //        SalesEmployee = sales.SalesEmployee.UserName,
        //        IMEI = sales.LinkedInventory == null? string.Empty : sales.LinkedInventory.IMEI,
        //    });
        //}
        public void Dispose()
        {
            this._dbContext.Dispose();
        }
    }
}
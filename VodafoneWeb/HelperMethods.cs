using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VodafoneWeb.Models;

namespace VodafoneWeb
{
    public static class HelperMethods
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        
        public static int? GetDealerId()
        {
            if (HttpContext.Current.Session["DealerId"] != null)
                return (int)HttpContext.Current.Session["DealerId"];
            else
                return null;
        }

        public static async Task<Dealer> GetCurrentDealer()
        {
            var dealerId = GetDealerId();

            if (dealerId.HasValue)
            {
                return await db.Dealers.FindAsync(dealerId);
            }
            return null;
        }

        public static void SetDealerId(int id)
        {
            HttpContext.Current.Session["DealerId"] = id;
        }

        public static void ClearDealerSelection()
        {
            HttpContext.Current.Session.Abandon();
        }

        public static List<SelectListItem> EnumToSelectList(Type enumType)
        {
            return Enum
              .GetValues(enumType)
              .Cast<int>()
              .Select(i => new SelectListItem
              {
                  Value = i.ToString(),
                  Text = Enum.GetName(enumType, i),
              }
              )
              .ToList();
        }

        public static string FormatFileName(string fileName)
        {
            if (fileName.EndsWith(".xlsx"))
            {
                if (HttpContext.Current.Session["DealerId"] != null)
                {
                    var dealerId = (int)HttpContext.Current.Session["DealerId"];
                    var dealer = db.Dealers.Find(dealerId);

                    if (dealer != null)
                    {
                        var temp = fileName.Substring(0, fileName.Length - 5);
                        return temp + " " + dealer.DealerName + ".xlsx";
                    }
                    else
                    {
                        return fileName;
                    }

                }
                else
                    return fileName;
            }
            else
            {
                return fileName;
            }
        }
    }
}
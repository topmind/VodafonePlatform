using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VodafoneWeb
{
    public static class HelperMethods
    {
        public static int? GetDealerId()
        {
            if (HttpContext.Current.Session["DealerId"] != null)
                return (int)HttpContext.Current.Session["DealerId"];
            else
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
    }
}
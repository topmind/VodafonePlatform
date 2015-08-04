using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VodafoneWeb
{
    public static class HelperMethods
    {
        public static string GetStoreId()
        {
            return (string)HttpContext.Current.Session["StoreId"];
        }

        public static void SetStoreId(string id)
        {
            HttpContext.Current.Session["StoreId"] = id;
        }
    }
}
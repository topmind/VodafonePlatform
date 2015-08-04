using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string IMEI { get; set; }
        public bool IsActive { get; set; }
    }
}
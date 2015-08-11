using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class Product
    {
        public Product()
        {
            IsActive = true;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        //public string IMEI { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
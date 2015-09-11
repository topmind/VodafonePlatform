using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            IsActive = true;
        }
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
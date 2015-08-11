using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class Category
    {
        public Category()
        {
            IsActive = true;
        }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}
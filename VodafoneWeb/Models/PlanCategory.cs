using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class PlanCategory
    {
        public PlanCategory()
        {
            IsActive = true;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}
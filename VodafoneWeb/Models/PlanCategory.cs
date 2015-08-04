using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class PlanCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}
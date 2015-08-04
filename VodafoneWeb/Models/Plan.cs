using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class Plan
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string OfferCode { get; set; }
        public bool IsActive { get; set; }

        public int PlanCategoryID { get; set; }

        public virtual PlanCategory PlanCategory { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class Plan
    {
        public Plan()
        {
            IsActive = true;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string OfferCode { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public int PlanCategoryID { get; set; }

        public virtual PlanCategory PlanCategory { get; set; }
    }
}
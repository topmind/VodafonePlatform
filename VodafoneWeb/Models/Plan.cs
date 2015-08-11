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
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public string OfferCode { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
    }
}
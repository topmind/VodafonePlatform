using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class Dealer
    {
        public Dealer()
        {
            IsActive = true;
        }
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<SalesTransaction> Sales { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; } 
    }
}
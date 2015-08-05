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
        public int ID { get; set; }
        public string Name { get; set; }
        public string DealerCode { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<SalesTransaction> Sales { get; set; }
    }
}
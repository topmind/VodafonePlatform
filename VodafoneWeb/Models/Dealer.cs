using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class Dealer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DealerCode { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<SalesTransaction> Sales { get; set; }
    }
}
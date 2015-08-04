using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class SalesTransaction
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MobileNumber { get; set; }
        public string Pin { get; set; }
        public int PlanID { get; set; }

        public virtual PlanCategory Category { get; set; }
        public virtual Plan Plan { get; set; }

        public int ProductID { get; set; }

        public virtual Product Product { get; set; }

        public int DealerID { get; set; }

        public virtual Dealer Dealer { get; set; }

        public int SalesEmployeeID { get; set; }

        public virtual ApplicationUser SalesEmployee { get; set; }
    }
}
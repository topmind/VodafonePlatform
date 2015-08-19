using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class SalesTransaction
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MobileNumber { get; set; }
        public string Pin { get; set; }
        public string OrderNumber { get; set; }

        public string PortinNumber { get; set; }

        //public int CategoryId { get; set; }
        public int PlanId { get; set; }
        public virtual Plan LinkedPlan { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        public int InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string RefferA { get; set; }

        public string RefferB { get; set; }

        public string Gift { get; set; }

        public string Note { get; set; }

        //public int InventoryID { get; set; }

        //public virtual Inventory LinkedInventory { get; set; }

        //public virtual PlanCategory Category { get; set; }
        //public virtual Plan Plan { get; set; }

        //public int ProductID { get; set; }

        //public virtual Product Product { get; set; }

        //public int DealerID { get; set; }

        //public virtual Dealer Dealer { get; set; }

        //public int SalesEmployeeID { get; set; }

        //public virtual ApplicationUser SalesEmployee { get; set; }

        //public virtual Inventory LinkedInventory { get; set; }

        //public string Reffer { get; set; }

        //public string Gift { get; set; }

        //public string Note { get; set; }
    }
}
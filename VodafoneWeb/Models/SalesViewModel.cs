using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodafoneWeb.Models
{
    public class SalesViewModel
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
        public SalesPlanViewModule Plan;

        public int CategoryId { get; set; }

        public SalesCategoryViewModule Category { get; set; }

        public string UserId { get; set; }

        public SalesUserViewModule User { get; set; }

        public int DealerId { get; set; }

        public DealerViewModel Dealer { get; set; }

        public int InventoryId { get; set; }

        public SalesInventoryViewModel Inventory { get; set; }

        public string IMEI { get; set; }

        public int ProductId { get; set; }

        public ProductViewModel Product { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string RefferA { get; set; }

        public string RefferB { get; set; }

        public string Gift { get; set; }

        public string Note { get; set; }
    }

}

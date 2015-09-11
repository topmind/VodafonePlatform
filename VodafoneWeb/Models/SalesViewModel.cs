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

        [Required(ErrorMessage = "Please input Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please input First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please input Mobile Number")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Please input Pin")]
        public string Pin { get; set; }

        [Required(ErrorMessage = "Please input Order Number")]
        public string OrderNumber { get; set; }

        public string PortinNumber { get; set; }
        //public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please select a plan")]
        [Display(Name = "Plan")]
        public int PlanId { get; set; }
        public SalesPlanViewModule Plan;

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public SalesCategoryViewModule Category { get; set; }

        public string UserId { get; set; }

        public SalesUserViewModule User { get; set; }

        public int DealerId { get; set; }

        public DealerViewModel Dealer { get; set; }

        public int? InventoryId { get; set; }

        public SalesInventoryViewModel Inventory { get; set; }

        public InventoryType? Type { get; set; }

        public string IMEI { get; set; }

        [Required(ErrorMessage = "Please select a product category")]
        [Display(Name = "Product Category")]
        public int ProductCategoryId { get; set; }

        public SalesProductCategoryViewModel ProductCategory { get; set; }

        [Required(ErrorMessage = "Please select a product")]
        public int ProductId { get; set; }

        public ProductViewModel Product { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string RefferA { get; set; }

        public string RefferB { get; set; }

        public string Gift { get; set; }

        public AuditType? Audit { get; set; }

        public string Note { get; set; }

        public bool IsChanged { get; set; }
    }

}

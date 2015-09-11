using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodafoneWeb.Models
{
    public class InventoryViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        public DateTime StockInDate { get; set; }
        public string StockInUserId { get; set; }
        public ApplicationUser StockInBy { get; set; }
        public string IMEI { get; set; }
        [Required(ErrorMessage = "Please select a product category")]
        [Display(Name = "Product Category")]
        public int ProductCategoryId { get; set; }
        public SalesProductCategoryViewModel ProductCategory { get; set; }
        [Required(ErrorMessage = "Please select a product")]
        public int ProductId { get; set; }
        public ProductViewModel Product { get; set; }

        public DateTime? StockOutDate { get; set; }
        public string StockOutUserId { get; set; }
        public ApplicationUser StockOutBy { get; set; }

        public string Note { get; set; }

        public InventoryOperationType Status { get; set; }

        public InventoryType Type { get; set; }

        public string PurchasedFrom { get; set; }

        public string PurchasedById { get; set; }

        public ApplicationUser PurchasedBy { get; set; }

        public string DefferCode { get; set; }

        public string DefferName { get; set; }

        public string DefferOrderNo { get; set; }

        public int DealerId { get; set; }

        public DealerViewModel Dealer { get; set; }

        public SalesViewModel RelatedSalesViewModel { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;

namespace VodafoneWeb.Models
{
    public enum InventoryOperationType
    {
        In,
        Out,
        TransferIn,
        TransferOut,
        Return
    }

    public enum InventoryType
    {
        Standard,
        ExchangeIMEI,
        Purchased,
        Deffer
    }
    public class Inventory
    {
        public Inventory()
        {
            Status = InventoryOperationType.In;
        }
        [ScaffoldColumn(false)]
        public int InventoryId { get; set; }

        public int DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        //[Index(IsUnique = true)]
        [MaxLength(450)]
        public string IMEI { get; set; }

        
        public string StockInById { get; set; }

        [ForeignKey("StockInById")]
        public virtual ApplicationUser StockInBy { get; set; }

        public DateTime StockInDate { get; set; }

        
        public string StockOutById { get; set; }
        [ForeignKey("StockOutById")]
        public virtual ApplicationUser StockOutBy { get; set; }

        public DateTime? StockOutDate { get; set; }

        public InventoryOperationType Status { get; set; }

        public InventoryType? Type { get; set; }

        public string PurchasedFrom { get; set; }

        public string PurchasedById { get; set; }

        public virtual ApplicationUser PurchasedBy { get; set; }

        public string DefferCode { get; set; }

        public string DefferName { get; set; }

        public string DefferOrderNo { get; set; }

        public string Note { get; set; }

    }

    public class InvetoryChangeHistory
    {
        [ScaffoldColumn(false)]
        public int InvetoryChangeHistoryId { get; set; }
        [MaxLength(450)]
        [Index]
        public string IMEI { get; set; }

        public DateTime ChangeDate { get; set; }

        public InventoryOperationType? OldOperationType { get; set; }

        public InventoryOperationType NewOperationType { get; set; }

        public string OperatedByEmployeeID { get; set; }

        public int? SalesId { get; set; }
        [ForeignKey("SalesId")]
        public virtual SalesTransaction Sales { get; set; }

        public string FromDealer { get; set; }

        public string ToDealer { get; set; }
    }
}

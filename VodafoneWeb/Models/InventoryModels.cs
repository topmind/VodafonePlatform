using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodafoneWeb.Models
{
    public enum InventoryOperationType
    {
        In,
        Out,
        Transfer
    }
    public class Inventory
    {
        public Inventory()
        {
            InStock = true;
        }
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        public virtual Dealer Dealer { get; set; }

        public virtual Product Product { get; set; }
        [Index(IsUnique = true)]
        public string IMEI { get; set; }

        public virtual ApplicationUser StockInBy { get; set; }

        public DateTime StockInDate { get; set; }

        public virtual ApplicationUser StockOutBy { get; set; }

        public DateTime? StockOutDate { get; set; }

        public bool InStock { get; set; }

    }

    public class InvetoryChangeHistory
    {
        public int ID { get; set; }

        public virtual Inventory Inventory { get; set; }

        public DateTime ChangeDate { get; set; }

        public InventoryOperationType OperationType { get; set; }

        public string OperatedByEmployeeID { get; set; }
    }
}

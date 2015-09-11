using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VodafoneWeb.Models
{
    public class InventoryChangeHistoryViewModel
    {
        [ScaffoldColumn(false)]
        public int InvetoryChangeHistoryId { get; set; }
        public string IMEI { get; set; }

        public DateTime ChangeDate { get; set; }

        public InventoryOperationType? OldOperationType { get; set; }

        public InventoryOperationType NewOperationType { get; set; }

        public string OperatedByEmployeeID { get; set; }

        public int? SalesId { get; set; }

        public SalesTransaction Sales { get; set; }

        public string FromDealer { get; set; }

        public string ToDealer { get; set; }
    }
}
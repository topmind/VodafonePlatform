﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodafoneWeb.Models
{
    public class SalesInventoryViewModel
    {
        public InventoryType? Type { get; set; }
        
        public int InventoryId { get; set; }

        public string IMEI { get; set; }

        public InventoryOperationType Status { get; set; }
    }
}

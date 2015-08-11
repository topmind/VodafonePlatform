﻿using System;
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
        //public int CategoryId { get; set; }
        public int PlanId { get; set; }
        public SalesPlanViewModule Plan;

        public int CategoryId { get; set; }

        public SalesCategoryViewModule Category { get; set; }
    }

}
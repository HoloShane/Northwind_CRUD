using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Northwind_CRUD.Models
{
    public class OrderModel
    {
        /// <summary>訂單編號</summary>
        public string OrderID { get; set; }

        /// <summary>訂購組織名稱</summary>
        public string CompanyName { get; set; }

        /// <summary>訂單日期</summary>
        public string OrderDate { get; set; }

        /// <summary>目的地</summary>
        public string ShipName { get; set; }

        /// <summary>目的地所在城市</summary>
        public string ShipCity { get; set; }
    }
}
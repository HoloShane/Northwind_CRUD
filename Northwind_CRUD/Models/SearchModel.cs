using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Northwind_CRUD.Models
{
    public class SearchModel
    {
        /// <summary>訂單編號</summary>
        public string OrderID { get; set; }

        /// <summary>訂購組織名稱</summary>
        public string CompanyName { get; set; }

        /// <summary>訂單日期(期間起)</summary>
        public string StartDate { get; set; }

        /// <summary>訂單日期(期間訖)</summary>
        public string EndDate { get; set; }
    }
}
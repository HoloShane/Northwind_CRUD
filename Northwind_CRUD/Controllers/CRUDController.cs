using Northwind_CRUD.Models;
using Northwind_CRUD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;

namespace Northwind_CRUD.Controllers
{
    public class CRUDController : Controller
    {
        /// <summary>
        /// 首頁(搜尋頁)
        /// </summary>
        /// <param name="SearchData">搜尋參數</param>
        /// <returns></returns>
        public ActionResult Index(SearchModel SearchData)
        {
            ViewBag.SearchData = new SearchModel();
            ViewBag.SearchData.StartDate = !string.IsNullOrEmpty(SearchData.StartDate) ? SearchData.StartDate : "";
            ViewBag.SearchData.EndDate = !string.IsNullOrEmpty(SearchData.EndDate) ? SearchData.EndDate : "";
            ViewBag.SearchData.OrderID = !string.IsNullOrEmpty(SearchData.OrderID) ? SearchData.OrderID : "";
            ViewBag.SearchData.CompanyName = !string.IsNullOrEmpty(SearchData.CompanyName) ? SearchData.CompanyName : "";
            return View();
        }

        /// <summary>
        /// 搜尋訂單
        /// </summary>
        /// <param name="filter">搜尋參數</param>
        /// <returns>訂單JSON物件</returns>
        public ActionResult SearchOrder(SearchModel filter)
        {
            try
            {
                string order_id = AntiXssEncoder.HtmlEncode(filter.OrderID, true);
                string company_name = AntiXssEncoder.HtmlEncode(filter.CompanyName, true);
                string start_date = AntiXssEncoder.HtmlEncode(filter.StartDate, true);
                string end_date = AntiXssEncoder.HtmlEncode(filter.EndDate, true);
                return Json(new CRUDRepository().Search(order_id, company_name, start_date, end_date), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// 新增頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {

            return View();
        }

        /// <summary>
        /// 修改頁
        /// </summary>
        /// <param name="OrderID">訂單編號</param>
        /// <returns>訂單內容</returns>
        public ActionResult Edit(string OrderID = "")
        {
            //"null"字串防呆
            if (OrderID == "null") OrderID = "";
            string order_id = AntiXssEncoder.HtmlEncode(OrderID, true);

            OrderModel order = new OrderModel();
            CRUDRepository rep = new CRUDRepository();
            order = rep.Search(order_id, "", "", "").FirstOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        /// <summary>
        /// 訂單修改
        /// </summary>
        /// <param name="temp_receiver"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(OrderModel temp_order)
        {
            if (ModelState.IsValid)
            {
                OrderModel order = new OrderModel();
                order.OrderID = AntiXssEncoder.HtmlEncode(temp_order.OrderID, true);
                order.CompanyName = AntiXssEncoder.HtmlEncode(temp_order.CompanyName, true);
                order.OrderDate = AntiXssEncoder.HtmlEncode(temp_order.OrderDate, true);
                order.ShipName = AntiXssEncoder.HtmlEncode(temp_order.ShipName, true);
                order.ShipCity = AntiXssEncoder.HtmlEncode(temp_order.ShipCity, true);

                CRUDRepository rep = new CRUDRepository();
                string rtn = rep.UpdateOrder(order);
                if (!string.IsNullOrEmpty(rtn))
                {
                    TempData["message"] = rtn;
                }
                return RedirectToAction("Index");
            }
            return View(temp_order);
        }

        /// <summary>
        /// 刪除頁
        /// </summary>
        /// <param name="OrderID">訂單編號</param>
        /// <returns>訂單內容</returns>
        public ActionResult Delete(string OrderID = "")
        {
            //"null"字串防呆
            if (OrderID == "null") OrderID = "";
            string order_id = AntiXssEncoder.HtmlEncode(OrderID, true);

            OrderModel receiver = new OrderModel();
            CRUDRepository rep = new CRUDRepository();
            receiver = rep.Search(order_id, "", "", "").FirstOrDefault();
            if (receiver == null)
            {
                return HttpNotFound();
            }
            return View(receiver);
        }

        /// <summary>
        /// 訂單刪除
        /// </summary>
        /// <param name="order">訂單編號</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(OrderModel order)
        {
            CRUDRepository rep = new CRUDRepository();
            string rtn = rep.DeleteOrder(order.OrderID);
            if (!string.IsNullOrEmpty(rtn))
            {
                TempData["message"] = rtn;
            }
            return RedirectToAction("Index");
        }
    }
}
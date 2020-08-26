using Dapper;
using Northwind_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Northwind_CRUD.Repository
{
    public class CRUDRepository
    {
        static readonly string DBConn = ConfigurationManager.ConnectionStrings["Northwind"].ToString();

        //搜尋訂單
        public List<OrderModel> Search(string OrderID, string CompanyName, string StartDate, string EndDate)
        {
            string SchCmd = $"SELECT OrderID, c.CompanyName, {Environment.NewLine}" +
            $"REPLACE(CONVERT(varchar, OrderDate, 111), '/', '-') OrderDate, {Environment.NewLine}" +
            $"ShipName, ShipCity FROM[Orders] o {Environment.NewLine}" +
            $"LEFT JOIN[Customers] c ON o.CustomerID = c.CustomerID {Environment.NewLine}";
            string QueryOrder = "ORDER BY OrderID";
            string QueryWhere = string.Empty;

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                QueryWhere += $"WHERE o.OrderDate BETWEEN '{StartDate} 00:00:00' AND '{EndDate} 23:59:59' {Environment.NewLine}";
            }
            else if (!string.IsNullOrEmpty(StartDate))
            {
                QueryWhere += $"WHERE o.OrderDate >= '{StartDate} 00:00:00' {Environment.NewLine}";
            }
            else if (!string.IsNullOrEmpty(EndDate))
            {
                QueryWhere += $"WHERE o.OrderDate <= '{EndDate} 23:59:59' {Environment.NewLine}";
            }

            if (!string.IsNullOrEmpty(OrderID))
            {
                QueryWhere += !string.IsNullOrEmpty(QueryWhere) ? $"AND OrderID LIKE '%{OrderID}%' {Environment.NewLine}" : $"WHERE OrderID LIKE '%{OrderID}%' {Environment.NewLine}";
            }
            if (!string.IsNullOrEmpty(CompanyName))
            {
                QueryWhere += !string.IsNullOrEmpty(QueryWhere) ? $"AND CompanyName LIKE '%{OrderID}%' {Environment.NewLine}" : $"WHERE CompanyName LIKE '%{OrderID}%' {Environment.NewLine}";
            }

            string CMD = SchCmd + QueryWhere + QueryOrder;

            using (SqlConnection dbConn = new SqlConnection(DBConn))
            {
                List<OrderModel> rtn = dbConn.Query<OrderModel>(CMD).ToList();
                return rtn;
            }
        }

        //更新訂單資訊
        public string UpdateOrder(OrderModel order)
        {
            using (SqlConnection dbConn = new SqlConnection(DBConn))
            {
                dbConn.Open();
                string UpdateQuery =
                    $"UPDATE Orders {Environment.NewLine}" +
                    $"SET OrderDate = @OrderDate, {Environment.NewLine}" +
                    $"ShipName = @ShipName, ShipCity = @ShipCity {Environment.NewLine}" +
                    $"WHERE OrderID = @OrderID";
                using (SqlCommand UpdateCmd = new SqlCommand(UpdateQuery, dbConn))
                {
                    UpdateCmd.Parameters.AddWithValue("@OrderID", ((object)order.OrderID) ?? DBNull.Value);
                    UpdateCmd.Parameters.AddWithValue("@OrderDate", ((object)order.OrderDate) ?? DBNull.Value);
                    UpdateCmd.Parameters.AddWithValue("@ShipName", ((object)order.ShipName) ?? DBNull.Value);
                    UpdateCmd.Parameters.AddWithValue("@ShipCity", ((object)order.ShipCity) ?? DBNull.Value);

                    try
                    {
                        UpdateCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException is UpdateException)
                        {
                            Console.WriteLine(ex.Message);
                            return "欲修改(或部分包含關鍵字)的收件者資訊已經存在！";
                        }
                        else
                        {
                            Console.WriteLine(ex.Message);
                            return ex.Message;
                        }
                    }
                    finally
                    {
                        dbConn.Close();
                    }
                }
            }
            return string.Empty;
        }

        //更新訂單資訊
        public string DeleteOrder(string order_id)
        {
            using (SqlConnection dbConn = new SqlConnection(DBConn))
            {
                dbConn.Open();
                string DeleteQuery =
                    $"DELETE [Order Details] WHERE OrderID = @OrderID {Environment.NewLine}" + 
                    $"DELETE [Orders] WHERE OrderID = @OrderID";
                using (SqlCommand DeleteCmd = new SqlCommand(DeleteQuery, dbConn))
                {
                    DeleteCmd.Parameters.AddWithValue("@OrderID", ((object)order_id) ?? DBNull.Value);

                    try
                    {
                        DeleteCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    finally
                    {
                        dbConn.Close();
                    }
                }
            }
            return string.Empty;
        }
    }
}
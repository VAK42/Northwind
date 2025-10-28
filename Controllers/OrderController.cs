using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class OrderController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var orders = new List<Orders>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM orders";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          orders.Add(new Orders
          {
            OrderId = Convert.ToInt32(row["order_id"]),
            CustomerId = row["customer_id"].ToString(),
            EmployeeId = Convert.ToInt32(row["employee_id"]),
            OrderDate = Convert.ToDateTime(row["order_date"]),
            RequiredDate = Convert.ToDateTime(row["required_date"]),
            ShippedDate = Convert.ToDateTime(row["shipped_date"]),
            ShipVia = Convert.ToInt32(row["ship_via"]),
            Freight = Convert.ToSingle(row["freight"]),
            ShipName = row["ship_name"].ToString(),
            ShipAddress = row["ship_address"].ToString(),
            ShipCity = row["ship_city"].ToString(),
            ShipRegion = row["ship_region"].ToString(),
            ShipPostalCode = row["ship_postal_code"].ToString(),
            ShipCountry = row["ship_country"].ToString()
          });
        }
      }

      return View(orders);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Orders order)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query =
            @"INSERT INTO orders (order_id, customer_id, employee_id, order_date, required_date, shipped_date, ship_via, freight, ship_name, ship_address, ship_city, ship_region, ship_postal_code, ship_country)
                        VALUES (@OrderId, @CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@OrderId", order.OrderId);
            cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
            cmd.Parameters.AddWithValue("@EmployeeId", order.EmployeeId);
            cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            cmd.Parameters.AddWithValue("@RequiredDate", order.RequiredDate);
            cmd.Parameters.AddWithValue("@ShippedDate", order.ShippedDate);
            cmd.Parameters.AddWithValue("@ShipVia", order.ShipVia);
            cmd.Parameters.AddWithValue("@Freight", order.Freight);
            cmd.Parameters.AddWithValue("@ShipName", order.ShipName);
            cmd.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
            cmd.Parameters.AddWithValue("@ShipCity", order.ShipCity);
            cmd.Parameters.AddWithValue("@ShipRegion", order.ShipRegion);
            cmd.Parameters.AddWithValue("@ShipPostalCode", order.ShipPostalCode);
            cmd.Parameters.AddWithValue("@ShipCountry", order.ShipCountry);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Order");
      }

      return View(order);
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
      Orders order = new Orders();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM orders WHERE order_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          order.OrderId = Convert.ToInt32(row["order_id"]);
          order.CustomerId = row["customer_id"].ToString();
          order.EmployeeId = Convert.ToInt32(row["employee_id"]);
          order.OrderDate = Convert.ToDateTime(row["order_date"]);
          order.RequiredDate = Convert.ToDateTime(row["required_date"]);
          order.ShippedDate = Convert.ToDateTime(row["shipped_date"]);
          order.ShipVia = Convert.ToInt32(row["ship_via"]);
          order.Freight = Convert.ToSingle(row["freight"]);
          order.ShipName = row["ship_name"].ToString();
          order.ShipAddress = row["ship_address"].ToString();
          order.ShipCity = row["ship_city"].ToString();
          order.ShipRegion = row["ship_region"].ToString();
          order.ShipPostalCode = row["ship_postal_code"].ToString();
          order.ShipCountry = row["ship_country"].ToString();
        }
      }

      return View(order);
    }

    [HttpPost]
    public ActionResult Edit(Orders model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query =
            @"UPDATE orders SET customer_id = @CustomerId, employee_id = @EmployeeId, order_date = @OrderDate, required_date = @RequiredDate, shipped_date = @ShippedDate, ship_via = @ShipVia, freight = @Freight, ship_name = @ShipName, ship_address = @ShipAddress, ship_city = @ShipCity, ship_region = @ShipRegion, ship_postal_code = @ShipPostalCode, ship_country = @ShipCountry WHERE order_id = @OrderId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@OrderId", model.OrderId);
            cmd.Parameters.AddWithValue("@CustomerId", model.CustomerId);
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@OrderDate", model.OrderDate);
            cmd.Parameters.AddWithValue("@RequiredDate", model.RequiredDate);
            cmd.Parameters.AddWithValue("@ShippedDate", model.ShippedDate);
            cmd.Parameters.AddWithValue("@ShipVia", model.ShipVia);
            cmd.Parameters.AddWithValue("@Freight", model.Freight);
            cmd.Parameters.AddWithValue("@ShipName", model.ShipName);
            cmd.Parameters.AddWithValue("@ShipAddress", model.ShipAddress);
            cmd.Parameters.AddWithValue("@ShipCity", model.ShipCity);
            cmd.Parameters.AddWithValue("@ShipRegion", model.ShipRegion);
            cmd.Parameters.AddWithValue("@ShipPostalCode", model.ShipPostalCode);
            cmd.Parameters.AddWithValue("@ShipCountry", model.ShipCountry);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Order");
      }

      return View(model);
    }

    public ActionResult Delete(int id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM orders WHERE order_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Order");
    }
  }
}
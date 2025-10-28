using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class OrderDetailController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var orderDetails = new List<OrderDetails>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM order_details";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          orderDetails.Add(new OrderDetails
          {
            OrderId = Convert.ToInt32(row["order_id"]),
            ProductId = Convert.ToInt32(row["product_id"]),
            UnitPrice = Convert.ToSingle(row["unit_price"]),
            Quantity = Convert.ToInt32(row["quantity"]),
            Discount = Convert.ToSingle(row["discount"])
          });
        }
      }

      return View(orderDetails);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(OrderDetails detail)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query =
            @"INSERT INTO order_details (order_id, product_id, unit_price, quantity, discount) VALUES (@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@OrderId", detail.OrderId);
            cmd.Parameters.AddWithValue("@ProductId", detail.ProductId);
            cmd.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
            cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
            cmd.Parameters.AddWithValue("@Discount", detail.Discount);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Orderdetail");
      }

      return View(detail);
    }

    [HttpGet]
    public ActionResult Edit(int orderId, int productId)
    {
      OrderDetails detail = new OrderDetails();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM order_details WHERE order_id = @orderId AND product_id = @productId";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@orderId", orderId);
        da.SelectCommand.Parameters.AddWithValue("@productId", productId);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          var row = dt.Rows[0];
          detail.OrderId = Convert.ToInt32(row["order_id"]);
          detail.ProductId = Convert.ToInt32(row["product_id"]);
          detail.UnitPrice = Convert.ToSingle(row["unit_price"]);
          detail.Quantity = Convert.ToInt32(row["quantity"]);
          detail.Discount = Convert.ToSingle(row["discount"]);
        }
      }

      return View(detail);
    }

    [HttpPost]
    public ActionResult Edit(OrderDetails detail)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query =
            @"UPDATE order_details SET unit_price = @UnitPrice, quantity = @Quantity, discount = @Discount WHERE order_id = @OrderId AND product_id = @ProductId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@OrderId", detail.OrderId);
            cmd.Parameters.AddWithValue("@ProductId", detail.ProductId);
            cmd.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
            cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
            cmd.Parameters.AddWithValue("@Discount", detail.Discount);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Orderdetail");
      }

      return View(detail);
    }

    public ActionResult Delete(int orderId, int productId)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM order_details WHERE order_id = @orderId AND product_id = @productId";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@orderId", orderId);
          cmd.Parameters.AddWithValue("@productId", productId);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Orderdetail");
    }
  }
}
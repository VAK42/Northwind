using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class ProductController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var products = new List<Products>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM products";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          products.Add(new Products
          {
            ProductId = Convert.ToInt32(row["product_id"]),
            ProductName = row["product_name"].ToString(),
            SupplierId = Convert.ToInt32(row["supplier_id"]),
            CategoryId = Convert.ToInt32(row["category_id"]),
            QuantityPerUnit = row["quantity_per_unit"].ToString(),
            UnitPrice = float.Parse(row["unit_price"].ToString()),
            UnitsInStock = Convert.ToInt32(row["units_in_stock"]),
            UnitsOnOrder = Convert.ToInt32(row["units_on_order"]),
            ReorderLevel = Convert.ToInt32(row["reorder_level"]),
            Discontinued = Convert.ToInt32(row["discontinued"])
          });
        }
      }

      return View(products);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Products product)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            INSERT INTO products 
            (product_id, product_name, supplier_id, category_id, quantity_per_unit, unit_price, units_in_stock, units_on_order, reorder_level, discontinued)
            VALUES 
            (@ProductId, @ProductName, @SupplierId, @CategoryId, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
            cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
            cmd.Parameters.AddWithValue("@SupplierId", product.SupplierId);
            cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
            cmd.Parameters.AddWithValue("@QuantityPerUnit", product.QuantityPerUnit);
            cmd.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            cmd.Parameters.AddWithValue("@UnitsInStock", product.UnitsInStock);
            cmd.Parameters.AddWithValue("@UnitsOnOrder", product.UnitsOnOrder);
            cmd.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
            cmd.Parameters.AddWithValue("@Discontinued", product.Discontinued);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Product");
      }

      return View(product);
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
      Products product = new Products();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM products WHERE product_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          product.ProductId = Convert.ToInt32(row["product_id"]);
          product.ProductName = row["product_name"].ToString();
          product.SupplierId = Convert.ToInt32(row["supplier_id"]);
          product.CategoryId = Convert.ToInt32(row["category_id"]);
          product.QuantityPerUnit = row["quantity_per_unit"].ToString();
          product.UnitPrice = float.Parse(row["unit_price"].ToString());
          product.UnitsInStock = Convert.ToInt32(row["units_in_stock"]);
          product.UnitsOnOrder = Convert.ToInt32(row["units_on_order"]);
          product.ReorderLevel = Convert.ToInt32(row["reorder_level"]);
          product.Discontinued = Convert.ToInt32(row["discontinued"]);
        }
      }

      return View(product);
    }

    [HttpPost]
    public ActionResult Edit(Products model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            UPDATE products 
            SET product_name = @ProductName,
                supplier_id = @SupplierId,
                category_id = @CategoryId,
                quantity_per_unit = @QuantityPerUnit,
                unit_price = @UnitPrice,
                units_in_stock = @UnitsInStock,
                units_on_order = @UnitsOnOrder,
                reorder_level = @ReorderLevel,
                discontinued = @Discontinued
            WHERE product_id = @ProductId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@ProductId", model.ProductId);
            cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
            cmd.Parameters.AddWithValue("@SupplierId", model.SupplierId);
            cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
            cmd.Parameters.AddWithValue("@QuantityPerUnit", model.QuantityPerUnit);
            cmd.Parameters.AddWithValue("@UnitPrice", model.UnitPrice);
            cmd.Parameters.AddWithValue("@UnitsInStock", model.UnitsInStock);
            cmd.Parameters.AddWithValue("@UnitsOnOrder", model.UnitsOnOrder);
            cmd.Parameters.AddWithValue("@ReorderLevel", model.ReorderLevel);
            cmd.Parameters.AddWithValue("@Discontinued", model.Discontinued);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Product");
      }

      return View(model);
    }

    public ActionResult Delete(int id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM products WHERE product_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Product");
    }
  }
}
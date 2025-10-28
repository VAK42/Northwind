using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class CategoryController : Controller
  {
    
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var categories = new List<Categories>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM categories";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          categories.Add(new Categories
          {
            CategoryId = Convert.ToInt32(row["category_id"]),
            CategoryName = row["category_name"].ToString(),
            Description = row["description"].ToString()
          });
        }
      }

      return View(categories);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Categories category)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            INSERT INTO categories 
            (category_id, category_name, description)
            VALUES 
            (@CategoryId, @CategoryName, @Description)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            cmd.Parameters.AddWithValue("@Description", category.Description);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Category");
      }

      return View(category);
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
      Categories category = new Categories();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM categories WHERE category_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          category.CategoryId = Convert.ToInt32(row["category_id"]);
          category.CategoryName = row["category_name"].ToString();
          category.Description = row["description"].ToString();
        }
      }

      return View(category);
    }

    [HttpPost]
    public ActionResult Edit(Categories model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            UPDATE categories 
            SET category_name = @CategoryName,
                description = @Description
            WHERE category_id = @CategoryId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
            cmd.Parameters.AddWithValue("@CategoryName", model.CategoryName);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Category");
      }

      return View(model);
    }

    public ActionResult Delete(int id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM categories WHERE category_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Category");
    }
  }
}
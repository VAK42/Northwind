using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class ShipperController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var shippers = new List<Shippers>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM shippers";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          shippers.Add(new Shippers
          {
            ShipperId = Convert.ToInt32(row["shipper_id"]),
            CompanyName = row["company_name"].ToString(),
            Phone = row["phone"].ToString()
          });
        }
      }

      return View(shippers);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Shippers shipper)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            INSERT INTO shippers 
            (shipper_id, company_name, phone)
            VALUES 
            (@ShipperId, @CompanyName, @Phone)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@ShipperId", shipper.ShipperId);
            cmd.Parameters.AddWithValue("@CompanyName", shipper.CompanyName);
            cmd.Parameters.AddWithValue("@Phone", shipper.Phone);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Shipper");
      }

      return View(shipper);
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
      Shippers shipper = new Shippers();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM shippers WHERE shipper_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          shipper.ShipperId = Convert.ToInt32(row["shipper_id"]);
          shipper.CompanyName = row["company_name"].ToString();
          shipper.Phone = row["phone"].ToString();
        }
      }

      return View(shipper);
    }

    [HttpPost]
    public ActionResult Edit(Shippers model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            UPDATE shippers 
            SET company_name = @CompanyName,
                phone = @Phone
            WHERE shipper_id = @ShipperId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@ShipperId", model.ShipperId);
            cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
            cmd.Parameters.AddWithValue("@Phone", model.Phone);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Shipper");
      }

      return View(model);
    }

    public ActionResult Delete(int id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM shippers WHERE shipper_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Shipper");
    }
  }
}
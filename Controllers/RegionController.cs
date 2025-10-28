using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class RegionController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var regions = new List<Region>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM region";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          regions.Add(new Region
          {
            RegionId = Convert.ToInt32(row["region_id"]),
            RegionDescription = row["region_description"].ToString()
          });
        }
      }

      return View(regions);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Region region)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            INSERT INTO region 
            (region_id, region_description)
            VALUES 
            (@RegionId, @RegionDescription)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@RegionId", region.RegionId);
            cmd.Parameters.AddWithValue("@RegionDescription", region.RegionDescription);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Region");
      }

      return View(region);
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
      Region region = new Region();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM region WHERE region_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          region.RegionId = Convert.ToInt32(row["region_id"]);
          region.RegionDescription = row["region_description"].ToString();
        }
      }

      return View(region);
    }

    [HttpPost]
    public ActionResult Edit(Region model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            UPDATE region 
            SET region_description = @RegionDescription
            WHERE region_id = @RegionId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@RegionId", model.RegionId);
            cmd.Parameters.AddWithValue("@RegionDescription", model.RegionDescription);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Region");
      }

      return View(model);
    }

    public ActionResult Delete(int id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM region WHERE region_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Region");
    }
  }
}
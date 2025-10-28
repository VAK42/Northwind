using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class TerritoryController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var territories = new List<Territories>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM territories";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          territories.Add(new Territories
          {
            TerritoryId = row["territory_id"].ToString(),
            TerritoryDescription = row["territory_description"].ToString(),
            RegionId = Convert.ToInt32(row["region_id"])
          });
        }
      }

      return View(territories);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Territories territory)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            INSERT INTO territories 
            (territory_id, territory_description, region_id)
            VALUES 
            (@TerritoryId, @TerritoryDescription, @RegionId)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@TerritoryId", territory.TerritoryId);
            cmd.Parameters.AddWithValue("@TerritoryDescription", territory.TerritoryDescription);
            cmd.Parameters.AddWithValue("@RegionId", territory.RegionId);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Territory");
      }

      return View(territory);
    }

    [HttpGet]
    public ActionResult Edit(string id)
    {
      var territory = new Territories();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM territories WHERE territory_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          territory.TerritoryId = row["territory_id"].ToString();
          territory.TerritoryDescription = row["territory_description"].ToString();
          territory.RegionId = Convert.ToInt32(row["region_id"]);
        }
      }

      return View(territory);
    }

    [HttpPost]
    public ActionResult Edit(Territories model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            UPDATE territories 
            SET territory_description = @TerritoryDescription,
                region_id = @RegionId
            WHERE territory_id = @TerritoryId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@TerritoryId", model.TerritoryId);
            cmd.Parameters.AddWithValue("@TerritoryDescription", model.TerritoryDescription);
            cmd.Parameters.AddWithValue("@RegionId", model.RegionId);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Territory");
      }

      return View(model);
    }

    public ActionResult Delete(string id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM territories WHERE territory_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Territory");
    }
  }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class SupplierController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var suppliers = new List<Suppliers>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM suppliers";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          suppliers.Add(new Suppliers
          {
            SupplierId = Convert.ToInt32(row["supplier_id"]),
            CompanyName = row["company_name"].ToString(),
            ContactName = row["contact_name"].ToString(),
            ContactTitle = row["contact_title"].ToString(),
            Address = row["address"].ToString(),
            City = row["city"].ToString(),
            Region = row["region"].ToString(),
            PostalCode = row["postal_code"].ToString(),
            Country = row["country"].ToString(),
            Phone = row["phone"].ToString(),
            Fax = row["fax"].ToString(),
            Homepage = row["homepage"].ToString()
          });
        }
      }

      return View(suppliers);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Suppliers supplier)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            INSERT INTO suppliers 
            (supplier_id, company_name, contact_name, contact_title, address, city, region, postal_code, country, phone, fax, homepage)
            VALUES 
            (@SupplierId, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax, @Homepage)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);
            cmd.Parameters.AddWithValue("@CompanyName", supplier.CompanyName);
            cmd.Parameters.AddWithValue("@ContactName", supplier.ContactName);
            cmd.Parameters.AddWithValue("@ContactTitle", supplier.ContactTitle);
            cmd.Parameters.AddWithValue("@Address", supplier.Address);
            cmd.Parameters.AddWithValue("@City", supplier.City);
            cmd.Parameters.AddWithValue("@Region", supplier.Region);
            cmd.Parameters.AddWithValue("@PostalCode", supplier.PostalCode);
            cmd.Parameters.AddWithValue("@Country", supplier.Country);
            cmd.Parameters.AddWithValue("@Phone", supplier.Phone);
            cmd.Parameters.AddWithValue("@Fax", supplier.Fax);
            cmd.Parameters.AddWithValue("@Homepage", supplier.Homepage);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Supplier");
      }

      return View(supplier);
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
      Suppliers supplier = new Suppliers();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM suppliers WHERE supplier_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          supplier.SupplierId = Convert.ToInt32(row["supplier_id"]);
          supplier.CompanyName = row["company_name"].ToString();
          supplier.ContactName = row["contact_name"].ToString();
          supplier.ContactTitle = row["contact_title"].ToString();
          supplier.Address = row["address"].ToString();
          supplier.City = row["city"].ToString();
          supplier.Region = row["region"].ToString();
          supplier.PostalCode = row["postal_code"].ToString();
          supplier.Country = row["country"].ToString();
          supplier.Phone = row["phone"].ToString();
          supplier.Fax = row["fax"].ToString();
          supplier.Homepage = row["homepage"].ToString();
        }
      }

      return View(supplier);
    }

    [HttpPost]
    public ActionResult Edit(Suppliers model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
            UPDATE suppliers 
            SET company_name = @CompanyName,
                contact_name = @ContactName,
                contact_title = @ContactTitle,
                address = @Address,
                city = @City,
                region = @Region,
                postal_code = @PostalCode,
                country = @Country,
                phone = @Phone,
                fax = @Fax,
                homepage = @Homepage
            WHERE supplier_id = @SupplierId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@SupplierId", model.SupplierId);
            cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName);
            cmd.Parameters.AddWithValue("@ContactName", model.ContactName);
            cmd.Parameters.AddWithValue("@ContactTitle", model.ContactTitle);
            cmd.Parameters.AddWithValue("@Address", model.Address);
            cmd.Parameters.AddWithValue("@City", model.City);
            cmd.Parameters.AddWithValue("@Region", model.Region);
            cmd.Parameters.AddWithValue("@PostalCode", model.PostalCode);
            cmd.Parameters.AddWithValue("@Country", model.Country);
            cmd.Parameters.AddWithValue("@Phone", model.Phone);
            cmd.Parameters.AddWithValue("@Fax", model.Fax);
            cmd.Parameters.AddWithValue("@Homepage", model.Homepage);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Supplier");
      }

      return View(model);
    }

    public ActionResult Delete(int id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM suppliers WHERE supplier_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Supplier");
    }
  }
}
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class CustomerController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var customers = new List<Customers>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM customers";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          customers.Add(new Customers
          {
            CustomerId = row["customer_id"].ToString(),
            CompanyName = row["company_name"].ToString(),
            ContactName = row["contact_name"].ToString(),
            ContactTitle = row["contact_title"].ToString(),
            Address = row["address"].ToString(),
            City = row["city"].ToString(),
            Region = row["region"].ToString(),
            PostalCode = row["postal_code"].ToString(),
            Country = row["country"].ToString(),
            Phone = row["phone"].ToString(),
            Fax = row["fax"].ToString()
          });
        }
      }

      return View(customers);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Customers customer)
    {
      if (ModelState.IsValid)
      {
        using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
                        INSERT INTO customers 
                        (customer_id, company_name, contact_name, contact_title, address, city, region, postal_code, country, phone, fax)
                        VALUES 
                        (@CustomerId, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
            cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
            cmd.Parameters.AddWithValue("@ContactName", customer.ContactName);
            cmd.Parameters.AddWithValue("@ContactTitle", customer.ContactTitle);
            cmd.Parameters.AddWithValue("@Address", customer.Address);
            cmd.Parameters.AddWithValue("@City", customer.City);
            cmd.Parameters.AddWithValue("@Region", customer.Region);
            cmd.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
            cmd.Parameters.AddWithValue("@Country", customer.Country);
            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
            cmd.Parameters.AddWithValue("@Fax", customer.Fax);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Customer");
      }

      return View(customer);
    }

    [HttpGet]
    public ActionResult Edit(string id)
    {
      Customers customer = new Customers();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM customers WHERE customer_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          customer.CustomerId = row["customer_id"].ToString();
          customer.CompanyName = row["company_name"].ToString();
          customer.ContactName = row["contact_name"].ToString();
          customer.ContactTitle = row["contact_title"].ToString();
          customer.Address = row["address"].ToString();
          customer.City = row["city"].ToString();
          customer.Region = row["region"].ToString();
          customer.PostalCode = row["postal_code"].ToString();
          customer.Country = row["country"].ToString();
          customer.Phone = row["phone"].ToString();
          customer.Fax = row["fax"].ToString();
        }
      }

      return View(customer);
    }

    [HttpPost]
    public ActionResult Edit(Customers model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"
                        UPDATE customers 
                        SET company_name = @CompanyName,
                            contact_name = @ContactName,
                            contact_title = @ContactTitle,
                            address = @Address,
                            city = @City,
                            region = @Region,
                            postal_code = @PostalCode,
                            country = @Country,
                            phone = @Phone,
                            fax = @Fax
                        WHERE customer_id = @CustomerId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@CustomerId", model.CustomerId);
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
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Customer");
      }

      return View(model);
    }

    public ActionResult Delete(string id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM customers WHERE customer_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Customer");
    }
  }
}
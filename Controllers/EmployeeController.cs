using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using Npgsql;
using VAK.Models;

namespace VAK.Controllers
{
  public class EmployeeController : Controller
  {
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      var employees = new List<Employees>();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM employees";
        var da = new NpgsqlDataAdapter(query, conn);
        var dt = new DataTable();
        da.Fill(dt);
        foreach (DataRow row in dt.Rows)
        {
          employees.Add(new Employees
          {
            EmployeeId = Convert.ToInt32(row["employee_id"]),
            LastName = row["last_name"].ToString(),
            FirstName = row["first_name"].ToString(),
            Title = row["title"].ToString(),
            TitleOfCourtesy = row["title_of_courtesy"].ToString(),
            BirthDate = Convert.ToDateTime(row["birth_date"]),
            HireDate = Convert.ToDateTime(row["hire_date"]),
            Address = row["address"].ToString(),
            City = row["city"].ToString(),
            Region = row["region"].ToString(),
            PostalCode = row["postal_code"].ToString(),
            Country = row["country"].ToString(),
            HomePhone = row["home_phone"].ToString(),
            Extension = row["extension"].ToString(),
            PhotoPath = row["photo_path"].ToString()
          });
        }
      }

      return View(employees);
    }

    [HttpGet]
    public ActionResult Create() => View();

    [HttpPost]
    public ActionResult Create(Employees employee)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"INSERT INTO employees 
                        (employee_id, last_name, first_name, title, title_of_courtesy, birth_date, hire_date, address, city, region, postal_code, country, home_phone, extension, photo_path)
                        VALUES 
                        (@EmployeeId, @LastName, @FirstName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate, @Address, @City, @Region, @PostalCode, @Country, @HomePhone, @Extension, @PhotoPath)";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@Title", employee.Title);
            cmd.Parameters.AddWithValue("@TitleOfCourtesy", employee.TitleOfCourtesy);
            cmd.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
            cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
            cmd.Parameters.AddWithValue("@Address", employee.Address);
            cmd.Parameters.AddWithValue("@City", employee.City);
            cmd.Parameters.AddWithValue("@Region", employee.Region);
            cmd.Parameters.AddWithValue("@PostalCode", employee.PostalCode);
            cmd.Parameters.AddWithValue("@Country", employee.Country);
            cmd.Parameters.AddWithValue("@HomePhone", employee.HomePhone);
            cmd.Parameters.AddWithValue("@Extension", employee.Extension);
            cmd.Parameters.AddWithValue("@PhotoPath", employee.PhotoPath);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Employee");
      }

      return View(employee);
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
      var employee = new Employees();
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        var query = "SELECT * FROM employees WHERE employee_id = @id";
        var da = new NpgsqlDataAdapter(query, conn);
        da.SelectCommand.Parameters.AddWithValue("@id", id);
        var dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
          DataRow row = dt.Rows[0];
          employee.EmployeeId = Convert.ToInt32(row["employee_id"]);
          employee.LastName = row["last_name"].ToString();
          employee.FirstName = row["first_name"].ToString();
          employee.Title = row["title"].ToString();
          employee.TitleOfCourtesy = row["title_of_courtesy"].ToString();
          employee.BirthDate = Convert.ToDateTime(row["birth_date"]);
          employee.HireDate = Convert.ToDateTime(row["hire_date"]);
          employee.Address = row["address"].ToString();
          employee.City = row["city"].ToString();
          employee.Region = row["region"].ToString();
          employee.PostalCode = row["postal_code"].ToString();
          employee.Country = row["country"].ToString();
          employee.HomePhone = row["home_phone"].ToString();
          employee.Extension = row["extension"].ToString();
          employee.PhotoPath = row["photo_path"].ToString();
        }
      }

      return View(employee);
    }

    [HttpPost]
    public ActionResult Edit(Employees model)
    {
      if (ModelState.IsValid)
      {
        using (var conn = new NpgsqlConnection(_connectionString))
        {
          conn.Open();
          var query = @"UPDATE employees 
                        SET last_name = @LastName,
                            first_name = @FirstName,
                            title = @Title,
                            title_of_courtesy = @TitleOfCourtesy,
                            birth_date = @BirthDate,
                            hire_date = @HireDate,
                            address = @Address,
                            city = @City,
                            region = @Region,
                            postal_code = @PostalCode,
                            country = @Country,
                            home_phone = @HomePhone,
                            extension = @Extension,
                            photo_path = @PhotoPath
                        WHERE employee_id = @EmployeeId";
          using (var cmd = new NpgsqlCommand(query, conn))
          {
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@LastName", model.LastName);
            cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@TitleOfCourtesy", model.TitleOfCourtesy);
            cmd.Parameters.AddWithValue("@BirthDate", model.BirthDate);
            cmd.Parameters.AddWithValue("@HireDate", model.HireDate);
            cmd.Parameters.AddWithValue("@Address", model.Address);
            cmd.Parameters.AddWithValue("@City", model.City);
            cmd.Parameters.AddWithValue("@Region", model.Region);
            cmd.Parameters.AddWithValue("@PostalCode", model.PostalCode);
            cmd.Parameters.AddWithValue("@Country", model.Country);
            cmd.Parameters.AddWithValue("@HomePhone", model.HomePhone);
            cmd.Parameters.AddWithValue("@Extension", model.Extension);
            cmd.Parameters.AddWithValue("@PhotoPath", model.PhotoPath);
            cmd.ExecuteNonQuery();
          }
        }

        return RedirectToAction("Index", "Employee");
      }

      return View(model);
    }

    public ActionResult Delete(int id)
    {
      using (var conn = new NpgsqlConnection(_connectionString))
      {
        conn.Open();
        var query = "DELETE FROM employees WHERE employee_id = @id";
        using (var cmd = new NpgsqlCommand(query, conn))
        {
          cmd.Parameters.AddWithValue("@id", id);
          cmd.ExecuteNonQuery();
        }
      }

      return RedirectToAction("Index", "Employee");
    }
  }
}
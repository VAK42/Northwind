using Npgsql;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace VAK.Controllers
{
  public class HomeController : Controller
  {
    private readonly string _conn = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

    public ActionResult Index()
    {
      if (Session["UserLoggedIn"] != null && (bool)Session["UserLoggedIn"])
      {
        return RedirectToAction("Index", "Customer");
      }

      return View();
    }

    [HttpPost]
    public ActionResult Index(string firstName, string lastName)
    {
      if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
      {
        ViewBag.ErrorMessage = "Please Enter First Name & Last Name!";
        return View();
      }

      var isValid = false;
      using (var connection = new NpgsqlConnection(_conn))
      {
        try
        {
          connection.Open();
          var query = "SELECT COUNT(1) FROM employees WHERE first_name = @firstName AND last_name = @lastName";
          using (var command = new NpgsqlCommand(query, connection))
          {
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@lastName", lastName);
            var count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0)
            {
              isValid = true;
            }
          }
        }
        catch (Exception ex)
        {
          ViewBag.ErrorMessage = "Error: " + ex.Message;
        }
      }

      if (isValid)
      {
        Session["UserLoggedIn"] = true;
        Session["FirstName"] = firstName;
        return RedirectToAction("Index", "Customer");
      }
      else
      {
        ViewBag.ErrorMessage = "Invalid Credentials!";
        return View();
      }
    }

    public ActionResult Logout()
    {
      Session["UserLoggedIn"] = null;
      Session["FirstName"] = null;
      return RedirectToAction("Index", "Home");
    }
  }
}
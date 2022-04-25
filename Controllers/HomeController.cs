using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Application.Models;

namespace Library_Application.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;
     
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {

            return View(new UserModel());
        }



        [HttpPost]
        public ActionResult Signup(UserModel userModel)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Insert Into [USER] Values (@Name, @Email)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", userModel.Name);
                cmd.Parameters.AddWithValue("@Email", userModel.Email);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Login()
        {

            return View(new UserModel());
        }



        [HttpPost]
        public ActionResult Login(UserModel userModel)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select * from [USER] where Email ='"+userModel.Email+"'";
               
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        TempData["User"] = reader.GetInt32(reader.GetOrdinal("UserID"));
                        return RedirectToAction("Index","User");
                    }
                    else
                    {
                        ViewBag.ErrMsg = "Invalid Email Please SignUp";

                        return View();
                    }
                }

            }


        }


        [HttpGet]
        public ActionResult Admin()
        {

            return View();
        }
    }
}
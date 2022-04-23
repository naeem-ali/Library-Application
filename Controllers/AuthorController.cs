using Library_Application.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Application.Controllers
{
    public class AuthorController : Controller
    {

        string connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;
        // GET: Author
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Author";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }

            return View(dt);
        }

        [HttpGet]
        // GET: Author/Create
        public ActionResult Create()
        {
            return View(new Author());
        }


        // POST: Author/Create
        [HttpPost]
        public ActionResult Create(Author authorModel)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Insert Into Author Values (@A_Name)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@A_Name", authorModel.A_Name);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Author/Edit/5
        public ActionResult Edit(int id)
        {
            Author authorModel = new Author();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Author where A_ID = @ID";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.SelectCommand.Parameters.AddWithValue("@ID", id);
                sda.Fill(dt);
            }
            if (dt.Rows.Count == 1)
            {
                authorModel.A_ID = id;
                authorModel.A_Name = dt.Rows[0][1].ToString();
             

                return View(authorModel);
            }
            else
                return RedirectToAction("Index");
        }

        // POST: Author/Edit/5
        [HttpPost]
        public ActionResult Edit(Author authorModel)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Update Author Set A_Name = @Name where A_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", authorModel.A_ID);
                cmd.Parameters.AddWithValue("@Name",authorModel.A_Name);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Author/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Delete from Author where A_ID = @ID ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

      
    }
}

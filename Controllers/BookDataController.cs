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
    public class BookDataController : Controller
    {

        string connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;
        // GET: BookData
        [HttpGet]
        public ActionResult Index()
        {
           DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Books";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dt);
            }

                return View(dt);
        }

       
        // GET: BookData/Create
        public ActionResult Create()
        {
            return View(new BookModel());
        }

        // POST: BookData/Create
        [HttpPost]
        public ActionResult Create(BookModel bookModel)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Insert Into Books Values (@Title, @Available, @Author)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Title", bookModel.Title);
                cmd.Parameters.AddWithValue("@Available", bookModel.Available);
                cmd.Parameters.AddWithValue("@Author", bookModel.Author);
             
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: BookData/Edit/5
        public ActionResult Edit(int id)
        {
            BookModel bookModel = new BookModel();
            DataTable dt = new DataTable();
            using(SqlConnection con =new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Books where ID = @ID";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.SelectCommand.Parameters.AddWithValue("@ID", id);
                sda.Fill(dt);
            }
            if(dt.Rows.Count == 1)
            {
                bookModel.ID = id;
                bookModel.Title = dt.Rows[0][1].ToString();
                bookModel.Available = Convert.ToInt32(dt.Rows[0][2].ToString());
                bookModel.Author = dt.Rows[0][3].ToString();

                return View(bookModel);
            }
           else 
                return RedirectToAction("Index");   
        }

        // POST: BookData/Edit/5
        [HttpPost]
      
        public ActionResult Edit(BookModel bookModel)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Update Books Set Title = @Title, Available = @Available, Author = @Author where ID = @ID ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", bookModel.ID);
                cmd.Parameters.AddWithValue("@Title", bookModel.Title);
                cmd.Parameters.AddWithValue("@Available", bookModel.Available);
                cmd.Parameters.AddWithValue("@Author", bookModel.Author);

                cmd.ExecuteNonQuery();
            }

                return RedirectToAction("Index");
        }

        // GET: BookData/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Delete from Books where ID = @ID ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

    }

}

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

    public class Authors
    {

        static string connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;

        public static List<Author> Getallauthorsusingbookid(int bookid)
        {
            List<Author> aList = new List<Author>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                String query = @"SELECT * FROM Books_Authors join Author on Author.A_ID= Books_Authors.A_ID  where B_ID=@bookId";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@bookId", bookid);
               

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {

                    while (reader.Read())
                    {
                        Author a = new Author();
                        a.A_ID = reader.GetInt32(reader.GetOrdinal("A_ID"));
                        a.A_Name = reader.GetString(reader.GetOrdinal("A_Name"));
                        aList.Add(a);
                    }
                }
            }

            return aList;
        }

        public static List<Author> Getall()
        {

            List<Author> aList = new List<Author>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Author";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = con;

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {

                    while (reader.Read())
                    {
                        Author a = new Author();
                        a.A_ID = reader.GetInt32(reader.GetOrdinal("A_ID"));
                        a.A_Name = reader.GetString(reader.GetOrdinal("A_Name"));
                        //author.Name = reader.GetString(reader.GetOrdinal("Name"));
                        //author.NameEmail = author.Name + " (" + author.Email + ")";
                        aList.Add(a);
                    }
                }
            }
            return aList;
        }
    }
}

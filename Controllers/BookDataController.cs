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
        public ActionResult Index(string CurrentSort)
        {
            List<BookModel> bList = new List<BookModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "Select * from Books";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = con;

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {

                    while (reader.Read())
                    {
                        BookModel b = new BookModel();
                        b.ID = reader.GetInt32(reader.GetOrdinal("Id"));
                        b.Title = reader.GetString(reader.GetOrdinal("Title"));
                        b.Available = reader.GetInt32(reader.GetOrdinal("Available"));
                        b.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                        bList.Add(b);
                    }
                }
                //SqlDataAdapter sda = new SqlDataAdapter(query, con);
                //sda.Fill(dt);

                if (CurrentSort == "Desc" || CurrentSort == null)
                {
                    ViewBag.CurrentSort = "Asc";
                    //CurrentSort = "Asc";
                    bList = bList.OrderBy(t => t.Title).ToList();
                }
                else
                {
                    ViewBag.CurrentSort = "Desc";
                    //CurrentSort = "Asc";
                    bList = bList.OrderByDescending(t => t.Title).ToList();
                }

            }
            return View(bList);
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
                string query = "Insert Into Books Values (@Title, @Available, @Quantity) SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Title", bookModel.Title);
                cmd.Parameters.AddWithValue("@Available", bookModel.Quantity);
                cmd.Parameters.AddWithValue("@Quantity", bookModel.Quantity);

                int bookid = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (int i in bookModel.Author)
                {
                    query = "Insert Into Books_Authors Values (@A_ID, @B_ID)";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@B_ID", bookid);
                    cmd.Parameters.AddWithValue("@A_ID", i);
                    
                    cmd.ExecuteNonQuery();
                }
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
                bookModel.Quantity = Convert.ToInt32(dt.Rows[0][3].ToString());

                //  bookModel.Author = dt.Rows[0][3].ToString();

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

                string query = "Select * from Books where ID=@ID";
                BookModel tempb = new BookModel();
                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@ID", bookModel.ID);

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {

                    while (reader.Read())
                    {
                        BookModel b = new BookModel();
                        tempb.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                        tempb.Available = reader.GetInt32(reader.GetOrdinal("Available"));
                        tempb.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));

                    }
                }
                con.Open();
                query = "Update Books Set Available=@Available, Quantity=@Quantity where ID = @ID ";
                 cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", bookModel.ID);

                int avail =(bookModel.Quantity -tempb.Quantity);
                if ((tempb.Available + avail) < 0)
                {

                }
                else
                {
                    cmd.Parameters.AddWithValue("@Available", tempb.Available + avail);
                    cmd.Parameters.AddWithValue("@Quantity", bookModel.Quantity);


                    cmd.ExecuteNonQuery();
                }
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
